using FabricWcf.DependencyInjection;
using FabricWcf.ServiceContracts;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Ninject;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;

namespace CoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CoreService : NInjectStatelessService
    {
        public CoreService(StatelessServiceContext context)
            : base(context)
        {
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return CreateInstanceListener<IClientService>();
            yield return CreateInstanceListener<IPassengerService>();
            yield return CreateInstanceListener<INewsService>();
        }

        private ServiceInstanceListener CreateInstanceListener<T>()
        {
            var contractName = typeof(T).CustomAttributes.First().NamedArguments.First(a => a.MemberName == "Name").TypedValue.Value;
            var instance = IoC.Kernel.Get<T>();
            return new ServiceInstanceListener(context => CreateListener(context, instance, contractName as string), instance.GetType().Name);
        }

        private WcfCommunicationListener<T> CreateListener<T>(ServiceContext context, T serviceInstance, string rootDir)
        {
            var binding = new WebHttpBinding("webHttpBinding"); //defined in App.config

            string host = context.NodeContext.IPAddressOrFQDN;
            var serviceUri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}/", "http", host, 8001, rootDir);
            var address = new EndpointAddress(serviceUri);

            var listener = new WcfCommunicationListener<T>(context, serviceInstance, binding, address);
            
            var endPoint = listener.ServiceHost.Description.Endpoints.Last();
            endPoint.EndpointBehaviors.Add(new WebHttpBehavior());

            return listener;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);
                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            }
        }
    }
}
