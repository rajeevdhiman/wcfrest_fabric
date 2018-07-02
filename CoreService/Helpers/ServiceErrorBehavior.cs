using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CoreService.Helpers
{
    public class ServiceErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        private readonly Type _errorHandlerType;

        public ServiceErrorBehaviorAttribute(Type errorHandlerType)
        {
            _errorHandlerType = errorHandlerType;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var errorHandler = (IErrorHandler)Activator.CreateInstance(_errorHandlerType);

            foreach (ChannelDispatcher channelDispatcher in
                serviceHostBase.ChannelDispatchers.Select(channelBase => channelBase as ChannelDispatcher))
                channelDispatcher.ErrorHandlers.Add(errorHandler);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}