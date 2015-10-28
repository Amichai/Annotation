using Annotation.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api {
    public class AnnotatedDocumentController : ApiController {
        public AnnotatedDocument Get(Guid id) {
            var annotations = AnnotationController.RandomAnnotations();
            return new AnnotatedDocument(annotations.OrderBy(i => i.TokenRange.StartIdx).ToList(),
                DocumentController.documents[0]);
        }
    }
}
