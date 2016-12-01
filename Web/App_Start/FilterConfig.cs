using System.Web;
using System.Web.Mvc;
using Web.AuthorizeAttr;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorWithLogAttribute());
        }
    }
}