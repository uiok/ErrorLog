﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLogProject.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string error)
        {
            return View();
        }
    }
}