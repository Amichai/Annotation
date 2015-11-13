using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class ProfileModel {
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public List<DocumentInfo> Documents { get; set; }
        public List<NewAnnotationModel> RecentAnnotations { get; set; }
        
    }
}