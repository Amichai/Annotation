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

        internal static DocumentInfo FromJson(string p) {
            throw new NotImplementedException();
        }

        internal static DocumentInfo FromDictionary(Dictionary<string, string> dict) {
            return new DocumentInfo() {
                Title = dict["Title"],
                Owner = dict["Owner"],
                AnnotationCount = int.Parse(dict["AnnotationCount"]),
                Id = Guid.Parse(dict["DocumentId"])
            };
        }
    }
}