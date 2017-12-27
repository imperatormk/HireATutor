using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HireATutor.Models;

namespace HireATutor.Filters
{
    public class WebApiAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(403);
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}