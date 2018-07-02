using System;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace CoreService.Helpers
{
    public class WcfServiceErrorHandler : IErrorHandler
    {
        /// <summary>
        /// This method will execute whenever any exception will be thrown from WCF
        /// </summary>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var faultException = new FaultException<Exception>(error);

            object detail = faultException.GetType().GetProperty("Detail").GetGetMethod().Invoke(faultException, null);

            fault = Message.CreateMessage(version, "", detail, new DataContractSerializer(detail.GetType()));

            var webBodyFormatMessageProp = new WebBodyFormatMessageProperty(WebContentFormat.Xml);

            fault.Properties.Add(WebBodyFormatMessageProperty.Name, webBodyFormatMessageProp);

            var httpResponseMessageProp = new HttpResponseMessageProperty();

            httpResponseMessageProp.Headers[HttpResponseHeader.ContentType] = "application/xml";
            httpResponseMessageProp.StatusCode = HttpStatusCode.BadRequest;
            httpResponseMessageProp.StatusDescription = error.Message.Replace(System.Environment.NewLine, string.Empty);

            fault.Properties.Add(HttpResponseMessageProperty.Name, httpResponseMessageProp);

            //IoC.Kernel.Get<IErrorLogManager>().LogError(error, HttpContext.Current, httpResponseMessageProp);
        }

        /// <summary>
        /// Performs error related behavior
        /// </summary>
        /// <param name="error">Exception raised by the program</param>
        /// <returns></returns>
        // Returning true indicates that an action(behavior) has been taken on the exception thrown.
        public bool HandleError(Exception error) => true;
    }
}