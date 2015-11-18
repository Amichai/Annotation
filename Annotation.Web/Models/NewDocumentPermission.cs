using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class NewDocumentPermission {
        public Guid DocumentId { get; set; }
        public string User { get; set; }
    }
}