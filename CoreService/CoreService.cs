using CoreService.Helpers;
using FabricWcf.DependencyInjection;
using FabricWCF.Common;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Ninject;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;

namespace CoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CoreService : NInjectFabricService
    {
        public CoreService(StatefulServiceContext context)
            : base(context)
        {
            ServiceUtility.StateManager = this.StateManager;
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            yield return CreateListener<ISetupService>();
            yield return CreateListener<INewsService>();
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");
                    var counter = await myDictionary.AddOrUpdateAsync(tx, "Counter", 1, (key, value) => ++value);
                    ServiceEventSource.Current.ServiceMessage(this.Context, "Working: {0}", counter);

                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
            }
        }

        private ServiceReplicaListener CreateListener<T>()
        {
            var contractName = typeof(T).GetCustomAttribute<ServiceContractAttribute>().Name;
            var instance = IoC.Kernel.Get<T>();
            return new ServiceReplicaListener(context => CreateListenerOfType(context, instance, contractName as string),  contractName as string);
        }

        private WcfCommunicationListener<T> CreateListenerOfType<T>(ServiceContext context, T serviceInstance, string rootDir)
        {
            var binding = new WebHttpBinding("webHttpBinding"); //defined in App.config
            var endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");

            string host = context.NodeContext.IPAddressOrFQDN;
            var serviceUri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}/", endpoint.Protocol, host, endpoint.Port, rootDir);
            var address = new EndpointAddress(serviceUri);

            var listener = new WcfCommunicationListener<T>(context, serviceInstance, binding, address); //"ServiceEndpoint");

            var endPoint = listener.ServiceHost.Description.Endpoints.Last();
            endPoint.EndpointBehaviors.Add(new WebHttpBehavior());

            return listener;
        }

        private ServiceReplicaListener CreateSoapListener<T>()
        {
            var contractName = typeof(T).GetCustomAttribute<ServiceContractAttribute>().Name;
            var instance = IoC.Kernel.Get<T>();
            return new ServiceReplicaListener(context => CreateSoapListener(context, instance, contractName as string), contractName as string);
        }

        private WcfCommunicationListener<T> CreateSoapListener<T>(ServiceContext context, T serviceInstance, string rootDir)
        {
            var binding = new BasicHttpBinding("basicHttpBinding");
            var endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");

            string host = context.NodeContext.IPAddressOrFQDN;
            var serviceUri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}/", endpoint.Protocol, host, endpoint.Port, "wsdl/" + rootDir);
            var address = new EndpointAddress(serviceUri);

            var listener = new WcfCommunicationListener<T>(context, serviceInstance, binding, address); //"ServiceEndpoint");
            ServiceMetadataBehavior smb = listener.ServiceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            // If not, add one
            if (smb == null)
            {
                smb = new ServiceMetadataBehavior();
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Default;
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = new Uri(serviceUri);

                listener.ServiceHost.Description.Behaviors.Add(smb);
            }
            return listener;
        }
    }
}
