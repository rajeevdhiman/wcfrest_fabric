using CoreService.Helpers;
using FabricWcf.ServiceContracts;
using FabricWcf.ServiceContracts.Objects;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace CoreService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [ServiceErrorBehavior(typeof(WcfServiceErrorHandler))]
    public class NewsService : INewsService
    {
        private static IEnumerable<FeedItem> GetRSS(string rssUrl)
        {
            using (XmlReader reader = XmlReader.Create(rssUrl))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                return ReadItems(feed);
            }
        }

        private static IEnumerable<FeedItem> ReadItems(SyndicationFeed feed)
        {
            foreach (var item in feed.Items)
            {
                var fi = new FeedItem
                {
                    Authors = item.Authors.Select(s => s.Name),
                    BaseUri = item.BaseUri?.AbsoluteUri,
                    Categories = item.Categories.Select(c => c.Name),
                    Contributors = item.Contributors.Select(c => c.Name),
                    Copyright = item.Copyright?.Text ?? string.Empty,
                    Id = item.Id,
                    Links = item.Links.Select(l => l.GetAbsoluteUri().AbsoluteUri),
                    PublishDate = item.PublishDate.DateTime,
                    Summary = item.Summary?.Text ?? string.Empty,
                    Title = item.Title?.Text ?? string.Empty,
                };
                yield return fi;
            }
        }

        public Task<IEnumerable<FeedItem>> Tech()
        {
            string feedUrl = "https://news.google.com/news/rss/headlines/section/topic/TECHNOLOGY.en_in/Technology?ned=in&hl=en-IN";
            return Task.Run(() => GetRSS(feedUrl));
        }

        public Task<IEnumerable<FeedItem>> Health()
        {
            string feedUrl = "https://news.google.com/news/rss/headlines/section/topic/SCIENCE.en_in/Science?ned=in&hl=en-IN";
            return Task.Run(() => GetRSS(feedUrl));
        }

        public Task<IEnumerable<FeedItem>> Science()
        {
            string feedUrl = "https://news.google.com/news/rss/headlines/section/topic/SCIENCE.en_in/Science?ned=in&hl=en-IN";
            return Task.Run(() => GetRSS(feedUrl));
        }
    }
}
