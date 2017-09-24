using System;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace WebApi2Book.Common.Security
{
    public class UserSession : IWebUserSession
    {
        public string FirstName => ((ClaimsPrincipal) HttpContext.Current.User)
            .FindFirst(ClaimTypes.GivenName)
            .Value;

        public string LastName => ((ClaimsPrincipal)HttpContext.Current.User)
            .FindFirst(ClaimTypes.Surname)
            .Value;

        public string UserName => ((ClaimsPrincipal)HttpContext.Current.User)
            .FindFirst(ClaimTypes.Name)
            .Value;

        public bool IsInRole(string roleName)
        {
            return HttpContext.Current.User.IsInRole(roleName);
        }

        public string ApiVersionInUse
        {
            get
            {
                const int versionIndex = 2;
                if (HttpContext.Current.Request.Url.Segments.Count() < versionIndex + 1)
                {
                    return string.Empty;
                }

                var apiVersionInUse = HttpContext.Current
                    .Request.Url.Segments[versionIndex].Replace("/", string.Empty);
                return apiVersionInUse;
            }
        }
        public Uri RequestUri => HttpContext.Current.Request.Url;
        public string HttpRequestMethod => HttpContext.Current.Request.HttpMethod;
    }
}
