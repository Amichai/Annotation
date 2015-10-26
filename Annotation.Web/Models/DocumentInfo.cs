using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class DocumentInfo {
        public string Title { get; set; }
        public string Owner { get; set; }
        public int AnnotationCount { get; set; }
        public Guid Id { get; set; }
    }
}