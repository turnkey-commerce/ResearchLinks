using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace ResearchLinks
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // using versioning in route to select controllers, e.g. v1, v2, etc.
            config.Routes.MapHttpRoute(
                name: "ProjectResearchItemsApi",
                routeTemplate: "api/v{version}/projects/{projectId}/researchItems/{researchItemId}",
                defaults: new { Controller = "ProjectResearchItems", researchItemId = RouteParameter.Optional }
            );
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v{version}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
