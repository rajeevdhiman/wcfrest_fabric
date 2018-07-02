using FabricWCF.Common;
using FabricWCF.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.ServiceClient.RESTful
{
    public class NewsClient : RESTClient<INewsService>, INewsService
    {
        public System.IO.Stream Data(string args) => this.Invoke((c) => c.Data(args));

        public async Task<List<FeedItem>> GetFeed(NewsCategory category, string lang = "en", string loc = "us")
        {
            return await this.InvokeAsync((c) => c.GetFeed(category, lang, loc));
        }

        public async Task<List<FeedItem>> Search(string query, string lang = "en", string loc = "us")
        {
            return await this.InvokeAsync((c) => c.Search(query, lang, loc));
        }
    }
}
