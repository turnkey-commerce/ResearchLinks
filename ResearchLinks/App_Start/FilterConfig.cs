using System.Web;
using System.Web.Mvc;
using ResearchLinks.Filters;
using System.Web.Http.Filters;

namespace ResearchLinks
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        //
        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ValidationFilterAttribute());
        }
    }
}