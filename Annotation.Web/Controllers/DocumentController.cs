﻿using Annotation.Web.Data;
using Annotation.Web.Models;
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
            bool canView, canAnnotate;
            if (user != null && user.IsAdministrator) {
                return View(new DocIdUserId(true, true) {
                    DocumentId = id,
                    UserId = user.UserId,
                    Role = user.Role
                });
            }
            if (user == null)
            {
                canView = true;
                canAnnotate = false;
            }
            else
            {
                DynamoDBConnection.Instance.UserDocumentPermissions(id, user.UserId, out canView, out canAnnotate);
            }
            var userId = user == null ? "" : user.UserId;
            var role = user == null ? "" : user.Role;

            return View(new DocIdUserId(canView, canAnnotate) {
                DocumentId = id,
                UserId = userId,
                Role = role
            });
        }
    }

    public class DocIdUserId {
        public DocIdUserId(bool canView, bool canAnnotate) {
            this.CanAnnotate = canAnnotate;
            this.CanView = canView;
        }
        public Guid DocumentId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public bool CanView { get; set; }
        public bool CanAnnotate { get; set; }
    }

    //TODO: User page, news feed
    //TODO: Search page, search within a document?
    //TODO: Document permissions
    //TODO: Star documents
    //One link per annotation
    //Request permission to edit
    //Date of post/upload
    //tooltip annotations (tags)
    //Link next chapter
    //Loading wheel while waiting for the document
    //pull right/left tool should hover and look like a button
}