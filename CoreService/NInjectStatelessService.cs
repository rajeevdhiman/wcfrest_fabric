using CoreService.Services;
using FabricWcf.DependencyInjection;
using FabricWcf.ServiceContracts;
using Microsoft.ServiceFabric.Services.Runtime;
using Ninject;
using Ninject.Extensions.Wcf;
using Ninject.Infrastructure;
using Ninject.Modules;
using System.Fabric;
using System.Reflection;

namespace CoreService
{
    public class NInjectStatelessService : StatelessService, IHaveKernel
    {
        public IKernel Kernel => IoC.Kernel;

        public NInjectStatelessService(StatelessServiceContext serviceContext) : base(serviceContext)
        {
            this.CreateKernel();
        }

        private IKernel CreateKernel()
        {
            IoC.Kernel = new StandardKernel(GetNinjectModules());
            IoC.Kernel.Load(Assembly.GetExecutingAssembly());
            return IoC.Kernel;
        }

        protected INinjectModule[] GetNinjectModules()
        {
            return new INinjectModule[] { new NinjectWcfModule() };
        }

        private class NinjectWcfModule : WcfModule
        {
            public override void Load()
            {
                Bind<IClientService>().To<ClientService>().InSingletonScope();
                Bind<IPassengerService>().To<PassengerService>().InSingletonScope();
                Bind<INewsService>().To<NewsService>().InSingletonScope();
            }
        }
    }
}
