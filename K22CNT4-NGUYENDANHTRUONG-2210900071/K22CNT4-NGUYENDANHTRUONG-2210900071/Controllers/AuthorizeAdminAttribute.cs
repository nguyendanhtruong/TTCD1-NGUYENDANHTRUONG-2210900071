using System;
using System.Web;
using System.Web.Mvc;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Filters
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["AdminID"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/NdtAdmins/NdtLogin");
        }
    }
}
