using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace WebApi2Book.Web.Common
{
    public class NamespaceHttpControllerSelector : IHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;

        public NamespaceHttpControllerSelector(HttpConfiguration config)
        {
            _configuration = config;
            _controllers = new Lazy<Dictionary<string, 
                HttpControllerDescriptor>>(InitializeControllerDictionary);
            
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string,
                HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            var assembliesResolver = _configuration.Services.GetAssembliesResolver();
            var controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (var controllerType in controllerTypes)
            {
                string[] segments = null;
                if (controllerType.Namespace != null)
                {
                    segments = controllerType.Namespace.Split(Type.Delimiter);
                }

                var controllerName =
                    controllerType.Name.Remove(controllerType.Name.Length -
                                               DefaultHttpControllerSelector.ControllerSuffix.Length);

                if (segments == null)
                    continue;

                var controllerKey = String.Format(CultureInfo.InvariantCulture,
                    "{0}.{1}", segments[segments.Length - 1], controllerName);

                if (!dictionary.Keys.Contains(controllerKey))
                {
                    dictionary[controllerKey] = new HttpControllerDescriptor(_configuration,
                        controllerType.Name,
                        controllerType);
                }
            }

            return dictionary;
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var controllerName = GetControllerName(routeData);
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var namespaceName = GetVersion(routeData);
            if (namespaceName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var controllerKey = string.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                namespaceName, controllerName);

            if (_controllers.Value.TryGetValue(controllerKey, out HttpControllerDescriptor controllerDescriptor))
            {
                return controllerDescriptor;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        private string GetVersion(IHttpRouteData routeData)
        {
            var subRouteData = routeData.GetSubRoutes().FirstOrDefault();
            if (subRouteData == null)
                return null;

            return GetRouteVariable<string>(subRouteData, "apiVersion");
        }

        private string GetControllerName(IHttpRouteData routeData)
        {
            var subroute = routeData.GetSubRoutes().FirstOrDefault();

            var dataTokenValue = subroute?.Route.DataTokens.First().Value;

            var controllerName =
                ((HttpActionDescriptor[]) dataTokenValue)?.First().ControllerDescriptor.ControllerName.Replace("Controller", string.Empty);
            return controllerName;
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllers.Value;
        }

        private T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            if (routeData.Values.TryGetValue(name, out object result))
            {
                return (T)result;
            }
            return default(T);
        }
    }
}
