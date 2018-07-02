using CoreService.Services;
using FabricWcf.DependencyInjection;
using FabricWCF.Common;
using Microsoft.ServiceFabric.Services.Runtime;
using Ninject;
using Ninject.Extensions.Wcf;
using Ninject.Infrastructure;
using System.Fabric;
using System.Reflection;

namespace CoreService
{
    public class NInjectFabricService : StatefulService, IHaveKernel
    {
        public NInjectFabricService(StatefulServiceContext serviceContext) : base(serviceContext)
        {
            this.CreateKernel();
        }

        public IKernel Kernel => IoC.Kernel;

        private IKernel CreateKernel()
        {
            IoC.Kernel = new StandardKernel(NinjectWcfModule.GetModule());
            IoC.Kernel.Load(Assembly.GetExecutingAssembly());
            return IoC.Kernel;
        }

        private class NinjectWcfModule : WcfModule
        {
            private NinjectWcfModule()
            {
            }

            public static NinjectWcfModule GetModule()
            {
                return new NinjectWcfModule();
            }

            public override void Load()
            {
                Bind<INewsService>().To<NewsService>().InSingletonScope();
                Bind<ITrelloService>().To<TrelloService>().InSingletonScope();
                Bind<ISetupService>().To<SetupService>().InSingletonScope();
            }
        }
    }
}