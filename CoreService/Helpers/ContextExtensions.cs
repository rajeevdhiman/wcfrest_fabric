using FabricWCF.Common.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreService
{
    public static class ContextExtensions
    {
        public static IEnumerable<HttpHeader> Headers(this OperationContext context)
        {
            if (context?.RequestContext == null)
                yield break;

            Message requestMessage = context.RequestContext.RequestMessage;
            HttpRequestMessageProperty httpRequestMessageProperty =
                requestMessage.Properties.Values.OfType<HttpRequestMessageProperty>().FirstOrDefault();

            if (httpRequestMessageProperty != null)
            {
                WebHeaderCollection httpHeaders = httpRequestMessageProperty.Headers;

                foreach (string header in httpHeaders.Keys)
                {
                    yield return new HttpHeader { Name = header, Value = httpHeaders.Get(header) };
                }
            }
        }

        public static string GetClientIP(this OperationContext context)
        {
            if (context?.IncomingMessageProperties == null)
                return string.Empty;

            MessageProperties props = context.IncomingMessageProperties;
            var clientEndpoint = props[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            return clientEndpoint != null ? clientEndpoint.Address : string.Empty;
        }
    }
}