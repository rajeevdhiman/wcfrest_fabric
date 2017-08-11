using Ninject;
using Ninject.Extensions.Wcf;

namespace FabricWcf.DependencyInjection
{
    public static class IoC
    {
        public static IKernel Kernel { get; set; }
    }
}
