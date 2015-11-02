using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class NewAnnotationModel {
        public string Body { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AnnotationId { get; set; }
    }
}