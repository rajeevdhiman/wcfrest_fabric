using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace FabricWcf.ServiceContracts
{
    [ServiceContract(Name = "Client")]
    public interface IClientService
    {
        [OperationContract]
        [WebGet(UriTemplate = "ClientName?id={id}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> ClientName(int id);

        [OperationContract]
        [WebGet(UriTemplate = "WhatIsMyIP", ResponseFormat = WebMessageFormat.Json)]
        string WhatIsMyIP();

        [OperationContract]
        [WebGet(UriTemplate = "MyHeaders", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> MyHeaders();
    }
}
