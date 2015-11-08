﻿using Annotation.Web.Models;
using Annotation.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annotation.Web.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index(Guid id)
        {
            var user = IdentityUtil.GetCurrentUser();
            return View(new DocIdUserId() {
                DocumentId = id, 
                UserId = user.UserId,
                Role = user.Role
            });
        }
    }

    public class DocIdUserId {
        public Guid DocumentId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}