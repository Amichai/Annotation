using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class AnnotationModel {
        public AnnotationModel(string author, string body, DocumentModel fullText) {
            this.Author = author;
            this.Body = body;
            this.setAnnotationBodyUnits(fullText);
        }

        private void setAnnotationBodyUnits(DocumentModel fullText) {
            this.AnnotationBodyUnits = new List<annotationBodyUnit>();
            var indices = this.backTickIndices();
            int lastIdx = 0;
            for (int i = 0; i < indices.Count; i += 2) {
                if (i == indices.Count - 1) {
                    break;
                }
                int idx1 = indices[i];
                int idx2 = indices[i + 1];
                string prefix = this.Body.Substring(lastIdx, idx1);
                this.AnnotationBodyUnits.Add(new annotationBodyUnit(prefix));
                string annotationString = this.Body.Substring(idx1, idx2).Trim('`');
                if (this.TokenRange == null) {
                    this.TokenRange = fullText.GetTokenRange(annotationString);
                }

                this.AnnotationBodyUnits.Add(new annotationBodyUnit(annotationString, true));
                lastIdx = idx2;
            }
            string suffix = string.Concat(this.Body.Skip(lastIdx));
            this.AnnotationBodyUnits.Add(new annotationBodyUnit(suffix));
        }

        public TokenRange TokenRange { get; private set; }
        public string Author { get; private set; }
        public string Body { get; private set; }

        private List<int> backTickIndices() {
            List<int> toReturn = new List<int>();
            int idx = -1;
            while (true) {
                idx = this.Body.IndexOf('`', idx + 1);
                if (idx == -1) {
                    break;
                }
                toReturn.Add(idx);
            }
            return toReturn;
        }

        public class annotationBodyUnit {
            public annotationBodyUnit(string val, bool linkedText = false) {
                this.Val = val.Trim('`');
                this.Type = linkedText ? "LinkedText" : "";
            }
            public string Type { get; set; }
            public string Val { get; set; }
        }

        public List<string> MyProperty { get; set; }
        public List<annotationBodyUnit> AnnotationBodyUnits { get; private set; }
    }
}