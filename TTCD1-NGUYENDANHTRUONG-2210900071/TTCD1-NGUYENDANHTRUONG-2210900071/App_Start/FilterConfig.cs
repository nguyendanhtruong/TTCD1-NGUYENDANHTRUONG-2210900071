using System.Web;
using System.Web.Mvc;

namespace TTCD1_NGUYENDANHTRUONG_2210900071
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
