using FabricWCF.Common;
using FabricWCF.Common.Objects;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Reader.ServiceClient
{
    public class NewsClient : ServicePartitionClient<WcfCommunicationClient<INewsService>>, INewsService
    {
        private static ICommunicationClientFactory<WcfCommunicationClient<INewsService>> communicationClientFactory;
        private static string endPointListnerName;

        static NewsClient()
        {
            var contractName = typeof(INewsService).CustomAttributes.First().NamedArguments.First(a => a.MemberName == "Name").TypedValue.Value;
            endPointListnerName = (string)contractName;
            var partitionResolver = new ServicePartitionResolver("codezilla.westus2.cloudapp.azure.com:19000", "codezilla.westus2.cloudapp.azure.com:19001");
            communicationClientFactory = new WcfCommunicationClientFactory<INewsService>(servicePartitionResolver: partitionResolver);
        }

        public NewsClient() : this(new Uri(@"fabric:/FabricWCF/CoreService"))
        {
        }

        public NewsClient(Uri serviceUri)
            : this(serviceUri, ServicePartitionKey.Singleton)
        {
        }

        public NewsClient(Uri serviceUri, ServicePartitionKey partitionKey)
            : base(communicationClientFactory, serviceUri, partitionKey, listenerName: endPointListnerName)
        {
        }

        public System.IO.Stream Data(string args) => this.InvokeWithRetry((c) => c.Channel.Data(args));

        public Task<List<FeedItem>> GetFeed(NewsCategory category, string lang = "en", string locale = "us")
        {
            return this.InvokeWithRetryAsync((c) => c.Channel.GetFeed(category, lang, locale));
        }
    }
}