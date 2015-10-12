﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeZones.Web.Controllers.api;
using TimeZones.Web.Data;
using TimeZones.Web.Util;

namespace TimeZones.Web.Controllers {
    public class UsersController : Controller {
        public ActionResult Index() {
            ViewBag.Title = "Users Page";
            bool isSessionValid = AuthenticationController.IsSessionIdValid(Request);
            if (!isSessionValid) {
                return RedirectToAction("Index", "Register");
            } else {
                var user = IdentityUtil.GetCurrentUser();
                if (!user.IsAdministrator && !user.IsManager) {
                    return RedirectToAction("Index", "Home");
                }
                var userId = Request.Cookies["user_id"].Value;
                ViewBag.UserId = userId;
                return View();
            }
        }
    }
}