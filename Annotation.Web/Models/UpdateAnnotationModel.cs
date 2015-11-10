using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class UpdateAnnotationModel {
        public Guid Id { get; set; }
        public string Body { get; set; }
    }
}