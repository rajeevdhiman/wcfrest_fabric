using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reader.ServiceClient.RESTful
{
    public abstract class RESTClient<TChannel>
    {
        private const string BASE_URL = "http://codezilla.westus2.cloudapp.azure.com/";
        //private const string BASE_URL = "http://localhost:21880/";

        public string ListnerUri
        {
            get
            {
                var contractName = typeof(TChannel).GetCustomAttribute<ServiceContractAttribute>().Name;
                return string.Format("{0}{1}", BASE_URL, contractName);
            }
        }

        private string BuildMethodUri(Delegate func)
        {
            var match = Regex.Match(func.Method.Name, @"\<(?<Name>.*)\>");
            var method = typeof(TChannel).GetMethod(match.Groups["Name"].Value);

            var requestUri = this.ListnerUri;
            if (Attribute.IsDefined(method, typeof(WebGetAttribute)))
            {
                var attribInfo = method.GetCustomAttribute<WebGetAttribute>();
                requestUri = string.Format("{0}/{1}", this.ListnerUri, attribInfo.UriTemplate);
            }
            else if (Attribute.IsDefined(method, typeof(WebInvokeAttribute)))
            {
                var attribInfo = method.GetCustomAttribute<WebInvokeAttribute>();
                requestUri = string.Format("{0}/{1}", this.ListnerUri, attribInfo.UriTemplate);
            }

            var args = method.GetParameters().ToList();

            args.ForEach(pi =>
            {
                requestUri = requestUri.Replace($"{{{pi.Name}}}", GetParameterValue(func.Target, pi.Name, pi.DefaultValue));
            });

            return requestUri;
        }

        private static string GetParameterValue(object paramObject, string argumentName, object defaultValue = null)
        {
            var argValue = (paramObject.GetType().GetField(argumentName)?.GetValue(paramObject) ?? defaultValue);
            return System.Web.HttpUtility.UrlEncode(argValue.ToString());
        }

        protected Task<TResult> InvokeAsync<TResult>(Func<TChannel, Task<TResult>> func)
        {
            var requestUri = BuildMethodUri(func);
            return ExecuteGetAsync<TResult>(requestUri);
        }

        protected Stream Invoke(Func<TChannel, Stream> func)
        {
            var requestUri = BuildMethodUri(func);
            return ExecuteGetStream(requestUri).Result;
        }

        private static async Task<K> ExecuteGetAsync<K>(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(requestUrl).ConfigureAwait(false);
                    return await response.Content.ReadAsAsync<K>().ConfigureAwait(false);
                }
                catch (Exception ex) { Console.Write(ex); return default(K); }
            }
        }

        private static async Task<Stream> ExecuteGetStream(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    return await client.GetStreamAsync(requestUrl);
                }
                catch (Exception ex) { Console.Write(ex); return default(Stream); }
            }
        }
        private static async Task<K> ExecutePostAsync<T, K>(string requestUrl, T postData)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var xmlFormatter = new XmlMediaTypeFormatter();
                    var dcs = new DataContractSerializer(typeof(T));
                    xmlFormatter.SetSerializer<T>(dcs);

                    var response = await client.PostAsync(requestUrl, postData, xmlFormatter).ConfigureAwait(false);
                    return await response.Content.ReadAsAsync<K>().ConfigureAwait(false);
                }
                catch (Exception ex) { Console.Write(ex); return default(K); }
            }
        }

        private static async Task<string> GetStringAsync(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(requestUrl).ConfigureAwait(false);
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (Exception ex) { return ex.ToString(); }
            }
        }
    }
}