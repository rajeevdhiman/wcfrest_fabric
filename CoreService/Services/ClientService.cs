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
    public class ClientService : IClientService
    {
        public ClientService() { }
       
        public Task<string> ClientName(int id)
        {
            return Task.Run(() => string.Format("Client {0}'s Name is :{1}", id, "Rajeev Kumar"));
        }

        public string WhatIsMyIP()
        {
            var context = OperationContext.Current;
            return string.Format("Your IP Address: {0}", context.GetClientIP());
        }

        public IEnumerable<string> MyHeaders()
        {
            var context = OperationContext.Current;
            return context.Headers();
        }
    }
}
