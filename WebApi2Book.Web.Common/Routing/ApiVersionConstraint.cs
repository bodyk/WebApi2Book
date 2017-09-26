using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace WebApi2Book.Web.Common.Routing
{
    public class ApiVersionConstraint : IHttpRouteConstraint
    {
        public ApiVersionConstraint(string allowedVersion)
        {
            AllowedVersion = allowedVersion.ToLowerInvariant();
        }

        public string AllowedVersion { get; set; }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            if (values.TryGetValue(parameterName, out object value) && value != null)
            {
                return AllowedVersion.Equals(value.ToString().ToLowerInvariant());
            }
            return false;
        }
    }
}
