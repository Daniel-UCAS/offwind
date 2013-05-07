﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Offwind.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.IsAdmin = Roles.GetRolesForUser(filterContext.HttpContext.User.Identity.Name).Contains("Admin");
            base.OnActionExecuted(filterContext);
        }
    }
}
