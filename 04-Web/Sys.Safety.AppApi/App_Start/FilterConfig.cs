using System.Web;
using System.Web.Mvc;

namespace Mas.KJ73N.AppApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
