using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace FabricWCF.Common
{
    [ServiceContract(Name = "Setup")]
    public interface ISetupService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Home", ResponseFormat = WebMessageFormat.Xml)]
        Task<string> Home();
    }
}
