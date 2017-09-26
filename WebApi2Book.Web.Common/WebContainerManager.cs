using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace WebApi2Book.Web.Common
{
    public class WebContainerManager
    {
        public static IDependencyResolver GetDependencyResolver()
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            if (dependencyResolver != null)
            {
                return dependencyResolver;
            }

            throw new InvalidOperationException("The dependency resolver has not been set.");
        }

        public static T Get<T>()
        {
            var service = GetDependencyResolver().GetService(typeof(T));
            if (service == null)
            {
                throw new NullReferenceException($"Requsted service of typr {typeof(T).FullName}," +
                                                 $" but null was found");
            }
            return (T) service;
        }

        public static IEnumerable<T> GetAll<T>()
        {
            var services = GetDependencyResolver().GetServices(typeof(T)).ToList();

            if (!services.Any())
            {
                throw new NullReferenceException($"Requsted services of typr {typeof(T).FullName}," +
                                                 $" but null was found");
            }
            return services.Cast<T>();
        }
    }
}
