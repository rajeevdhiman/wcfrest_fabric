using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.ServiceModel.Web;

namespace CoreService.Helpers
{
    public static class ServiceUtility
    {
        public static IReliableStateManager StateManager { get; set; }

        /// <summary>
        /// Retrieves data from API resource using HTTP GET method, and caches the content for later use.
        /// </summary>
        /// <typeparam name="T">Return Type on method result, used to de-serialize API response</typeparam>
        /// <param name="requestUrl">RESTful API url</param>
        /// <param name="cacheAge">Accepted cache data age in minutes, default 300 minutes</param>
        /// <returns>Object of Type</returns>
        public static async Task<T> ExecuteGetAsync<T>(string requestUrl, int cacheAge = 300)
        {
            var httpCache = await StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("httpCache");
            var httpCacheTime = await StateManager.GetOrAddAsync<IReliableDictionary<string, DateTime>>("httpCacheTime");

            using (var tx = StateManager.CreateTransaction())
            {
                var content = await httpCache.TryGetValueAsync(tx, requestUrl);
                var cacheTime = await httpCacheTime.TryGetValueAsync(tx, requestUrl);

                if (content.HasValue && cacheTime.HasValue && cacheTime.Value.Subtract(DateTime.UtcNow).TotalMinutes < cacheAge)
                {
                    return JsonConvert.DeserializeObject<T>(content.Value);
                }
            }

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonText = await client.GetStringAsync(requestUrl).ConfigureAwait(false);

                    using (var tx = StateManager.CreateTransaction())
                    {
                        await httpCache.AddOrUpdateAsync(tx, requestUrl, jsonText, (key, value) => jsonText);
                        await httpCacheTime.AddOrUpdateAsync(tx, requestUrl, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
                        await tx.CommitAsync();
                    }

                    return JsonConvert.DeserializeObject<T>(jsonText);
                }
                catch (Exception ex) { Console.Write(ex); return default(T); }
            }
        }

        public static async Task<string> GetStringAsync(string requestUrl, int cacheAge = 300)
        {
            var httpCache = await StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("httpCache");
            var httpCacheTime = await StateManager.GetOrAddAsync<IReliableDictionary<string, DateTime>>("httpCacheTime");

            using (var tx = StateManager.CreateTransaction())
            {
                var content = await httpCache.TryGetValueAsync(tx, requestUrl);
                var cacheTime = await httpCacheTime.TryGetValueAsync(tx, requestUrl);

                if (content.HasValue && cacheTime.HasValue && cacheTime.Value.Subtract(DateTime.UtcNow).TotalMinutes < cacheAge)
                {
                    return content.Value;
                }
            }

            using (var client = new HttpClient())
            {
                try
                {
                    var resultText = await client.GetStringAsync(requestUrl).ConfigureAwait(false);

                    using (var tx = StateManager.CreateTransaction())
                    {
                        await httpCache.AddOrUpdateAsync(tx, requestUrl, resultText, (key, value) => resultText);
                        await httpCacheTime.AddOrUpdateAsync(tx, requestUrl, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
                        await tx.CommitAsync();
                    }

                    return resultText;
                }
                catch (Exception ex) { Console.Write(ex); return default(string); }
            }
        }

        internal static async Task<Stream> GetStreamAsync(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(requestUrl).ConfigureAwait(false);
                    var contentType = result.Content.Headers.ContentType.ToString();
                    var mem = new MemoryStream();
                    var content = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    content.CopyTo(mem);
                    return mem;
                }
                catch (Exception ex) { Console.Write(ex); return default(System.IO.Stream); }
            }
        }

        internal static Stream GetStream(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = client.GetAsync(requestUrl).Result;
                    WebOperationContext.Current.OutgoingResponse.ContentType = result.Content.Headers.ContentType.ToString();
                    var mem = new MemoryStream();
                    var content = result.Content.ReadAsStreamAsync().Result;
                    content.CopyTo(mem);
                    mem.Seek(0, SeekOrigin.Begin);
                    return mem;
                }
                catch (Exception ex) { Console.Write(ex); return default(System.IO.Stream); }
            }
        }
    }
}
