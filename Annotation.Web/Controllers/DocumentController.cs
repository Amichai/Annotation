using Annotation.Web.Models;
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
            return View(id);
        }
    }
}