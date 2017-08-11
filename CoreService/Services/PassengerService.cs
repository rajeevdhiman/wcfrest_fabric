using CoreService.Helpers;
using FabricWcf.ServiceContracts;
using System.Collections.Generic;
using System.Fabric;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CoreService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [ServiceErrorBehavior(typeof(WcfServiceErrorHandler))]
    public class PassengerService : IPassengerService
    {
        public PassengerService() { }

        public Task<string> PassengerName(int id)
        {
            return Task.Run(() => string.Format("PassengerName:{0}", "Rajeev Kumar"));
        }

        public string MyIP()
        {
            var context = OperationContext.Current;
            return string.Format("Your IP Address: {0}", context.GetClientIP());
        }

        public IEnumerable<string> GetAll()
        {
            var context = OperationContext.Current;
            return context.Headers();
        }
    }
}
