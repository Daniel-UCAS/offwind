﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Offwind.WebApp.Models;

namespace Offwind.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected OffwindEntities _ctx = new OffwindEntities();

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.Version = WebConfigurationManager.AppSettings["AppVersion"];
            ViewBag.IsAdmin = Roles.GetRolesForUser(filterContext.HttpContext.User.Identity.Name).Contains("Admin");
            base.OnActionExecuted(filterContext);
        }
    }
}
