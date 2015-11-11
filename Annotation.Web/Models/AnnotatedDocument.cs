using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class AnnotatedDocument {
        public AnnotatedDocument(List<AnnotationModel> annotations, DocumentModel document) {
            this.Annotations = annotations;
            this.Document = document;
            this.Document.ClearLinkedAnnotations();
            this.Annotators = this.Annotations.Select(i => i.Author).Distinct().ToList();
            for (int i = 0; i < this.Annotations.Count; i++) {
                var annotation = this.Annotations[i];
                if (annotation.TokenRange == null) {
                    continue;
                }
                int start = annotation.TokenRange.StartIdx;
                int range = annotation.TokenRange.Range;
                for(int j = start; j < start + range; j++) {
                    if (this.Document.Tokens[j].LinkedAnnotations.Contains(i)) {
                        continue;
                    }
                    this.Document.Tokens[j].LinkedAnnotations.Add(i);
                    var a = this.Document.Tokens[j].TokenVal;
                    var b = annotation.Body;
                }
			}
        }

        public List<string> Annotators { get; set; }
        public DocumentModel Document { get; set; }
        public List<AnnotationModel> Annotations { get; set; }
    }
}