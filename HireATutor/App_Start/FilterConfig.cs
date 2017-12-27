using System.Web;
using System.Web.Mvc;
using HireATutor.Filters;

namespace HireATutor
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
