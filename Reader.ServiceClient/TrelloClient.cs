using FabricWCF.Common;
using FabricWCF.Common.Trello;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Reader.ServiceClient
{
    public class TrelloClient : ServicePartitionClient<WcfCommunicationClient<ITrelloService>>, ITrelloService
    {
        private static ICommunicationClientFactory<WcfCommunicationClient<ITrelloService>> communicationClientFactory;
        private static string endPointListnerName;

        static TrelloClient()
        {
            var contractName = typeof(ITrelloService).CustomAttributes.First().NamedArguments.First(a => a.MemberName == "Name").TypedValue.Value;
            endPointListnerName = (string)contractName;

            communicationClientFactory = new WcfCommunicationClientFactory<ITrelloService>();
        }
        public TrelloClient() : this(new Uri(@"fabric:/FabricWCF/CoreService"))
        {
        }
        public TrelloClient(Uri serviceUri)
            : this(serviceUri, ServicePartitionKey.Singleton)
        {
        }

        public TrelloClient(Uri serviceUri, ServicePartitionKey partitionKey)
            : base(communicationClientFactory, serviceUri, partitionKey, listenerName: endPointListnerName)
        {
        }

        public Task<Card2> CardInformation(string id) => this.InvokeWithRetryAsync((c) => c.Channel.CardInformation(id));

        public Task<List<Card2>> CardSearch(string name) => this.InvokeWithRetryAsync((c) => c.Channel.CardSearch(name));

        public Task<List<string>> GetCardInfos(string cardNames) => this.InvokeWithRetryAsync((c) => c.Channel.GetCardInfos(cardNames));

        public Task<List<CardList>> GetLists()=> this.InvokeWithRetryAsync((c) => c.Channel.GetLists());
        
    }
}