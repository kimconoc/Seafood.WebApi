using System.Web;
using System.Web.Mvc;

namespace Amin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute {
                View = "_Error"
            });
        }
    }
}
