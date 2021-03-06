﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Annotation.Web.Controllers.api;
using Annotation.Web.Data;

namespace Annotation.Web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index(string userId = null) {
            ViewBag.Title = "Home Page";
            var sessionId = Request.Cookies["session_id"];
            bool isSessionValid = AuthenticationController.IsSessionIdValid(Request);
            if (!isSessionValid) {
                return RedirectToAction("Index", "Register");
            } 
            if (userId == null) {
                userId = Request.Cookies["user_id"].Value;
                ViewBag.UserId = userId;
            }
            return View();
        }
    }
}
