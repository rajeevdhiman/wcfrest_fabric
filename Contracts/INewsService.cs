using FabricWcf.ServiceContracts.Objects;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace FabricWcf.ServiceContracts
{
    [ServiceContract(Name = "News")]
    public interface INewsService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Tech", ResponseFormat = WebMessageFormat.Xml)]
        Task<IEnumerable<FeedItem>> Tech();

        [OperationContract]
        [WebGet(UriTemplate = "Science", ResponseFormat = WebMessageFormat.Xml)]
        Task<IEnumerable<FeedItem>> Science();
    }
}
