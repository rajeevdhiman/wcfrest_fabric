using CoreService.Helpers;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CoreService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [ServiceErrorBehavior(typeof(WcfServiceErrorHandler))]
    internal abstract class WcfServiceBase
    {
        public WebOperationContext Context => WebOperationContext.Current;
    }
}