﻿using System.Web.Mvc;
using Offwind.WebApp.Models;

namespace Offwind.WebApp.Areas.Help.Controllers
{
    public class HomeHelpController : _BaseController
    {
        public ActionResult Index()
        {
            var m = new VWebPage();
            ViewBag.Title = "Help | Offwind";
            return View(m);
        }

    }
}
