using FabricWCF.Common;
using FabricWCF.Common.Objects;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System;
using System.Text;
using CoreService.Helpers;
using System.ServiceModel.Web;
using System.IO;

namespace CoreService.Services
{
    internal class NewsService : WcfServiceBase, INewsService
    {
        public Task<List<FeedItem>> GetFeed(NewsCategory category, string lang = "en", string locale = "us")
        {
            string feedUrl = $"https://news.google.com/news/rss/headlines?ned={locale}";
            if (category != NewsCategory.Headlines)
            {
                var catName = category.ToString();
                feedUrl = $"https://news.google.com/news/rss/headlines/section/topic/{catName.ToUpper()}.{lang}_{locale}/{catName}?ned={locale}&hl={lang}-{locale.ToUpper()}";
            }
            return Task.FromResult(ReadRSS(feedUrl));
        }

        public Task<List<FeedItem>> Search(string query, string lang = "en", string locale = "us")
        {
            string feedUrl = $"https://news.google.com/news/rss/search/section/q/{query}/?hl={lang}-{locale.ToUpper()}&ned={locale}";
            return Task.FromResult(ReadRSS(feedUrl));
        }

        private static IEnumerable<FeedItem> ReadItems(SyndicationFeed feed)
        {
            foreach (var item in feed.Items)
            {
                var fi = new FeedItem
                {
                    Authors = item.Authors.Select(s => s.Name).ToList(),
                    BaseUri = item.BaseUri?.AbsoluteUri,
                    Categories = item.Categories.Select(c => c.Name).ToList(),
                    Contributors = item.Contributors.Select(c => c.Name).ToList(),
                    Copyright = item.Copyright?.Text ?? string.Empty,
                    Id = item.Id,
                    Links = item.Links.Select(l => l.GetAbsoluteUri().AbsoluteUri).ToList(),
                    PublishDate = item.PublishDate.DateTime,
                    Summary = item.Summary?.Text ?? string.Empty,
                    Title = item.Title?.Text ?? string.Empty,
                };
                yield return fi;
            }
        }

        private static List<FeedItem> ReadRSS(string rssUrl)
        {
            using (XmlReader reader = XmlReader.Create(rssUrl))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                return ReadItems(feed).ToList();
            }
        }

        public System.IO.Stream Data(string args)
        {
            var dataUri = Convert.FromBase64String(args);
            var requestUrl = Encoding.UTF8.GetString(dataUri);
            string textContent;

            try
            {
                var reader = new ReadSharp.Reader();
                var article = reader.Read(new Uri(requestUrl)).Result;
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html; charset=utf-8";
                textContent = string.Format(@"<!DOCTYPE html>
                                    <html lang='en'>
                                    <head>
                                        <meta charset='utf-8'>
                                        <title>{0}</title>
                                    </head>
                                    <body>{1}</body>
                                    </html>", article.Title, article.Content);
            }
            catch (ReadSharp.ReadException exc)
            {
                textContent = exc.ToString();
            }

            var memStream = new MemoryStream();
            var byteArr = Encoding.UTF8.GetBytes(textContent);
            memStream.Write(byteArr, 0, byteArr.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
    }
}