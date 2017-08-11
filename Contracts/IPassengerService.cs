using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace FabricWcf.ServiceContracts
{
    [ServiceContract(Name = "Passenger")]
    public interface IPassengerService
    {
        [OperationContract]
        [WebGet(UriTemplate = "PassengerName?id={id}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> PassengerName(int id);

        [OperationContract]
        [WebGet(UriTemplate = "GetAll", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> GetAll();
    }
}
