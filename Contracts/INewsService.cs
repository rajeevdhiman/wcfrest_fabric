using FabricWCF.Common.Objects;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace FabricWCF.Common
{
    [ServiceContract(Name = "News")]
    public interface INewsService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetFeed?category={category}&lang={lang}&loc={loc}", ResponseFormat = WebMessageFormat.Xml)]
        Task<List<FeedItem>> GetFeed(NewsCategory category, string lang = "en", string loc = "us");

        [OperationContract]
        [WebGet(UriTemplate = "Search?query={query}&lang={lang}&loc={loc}", ResponseFormat = WebMessageFormat.Xml)]
        Task<List<FeedItem>> Search(string query, string lang = "en", string loc = "us");

        [OperationContract]
        [WebGet(UriTemplate = "Data?args={args}")]
        Stream Data(string args);
    }
}
