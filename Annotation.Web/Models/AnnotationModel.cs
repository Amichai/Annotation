using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class AnnotationModel {
        private const int PREVIEW_LENGTH = 200;
        public AnnotationModel(string author, string body, DocumentModel fullText) {
            this.Author = author;
            this.Body = body;
            if (this.Body.Length <= PREVIEW_LENGTH) {
                this.PreviewText = this.Body;
            } else {
                this.PreviewText = this.Body.Substring(0, 200) + "...";
            }
            this.setAnnotationBodyUnits(fullText);
            this.IsStarred = false;
        }

        public bool IsStarred { get; set; }

        public bool IsPreviewCutoff {
            get {
                return this.AnnotationBodyUnits.CharLength - 1 > this.AnnotationPreviewUnits.CharLength;
            }
        }

        private void setAnnotationBodyUnits(DocumentModel fullText) {
            this.AnnotationBodyUnits = new TokenizedAnnotation();
            var indices = this.backTickIndices();
            int lastIdx = 0;
            for (int i = 0; i < indices.Count; i += 2) {
                if (i == indices.Count - 1) {
                    break;
                }
                int idx1 = indices[i];
                int idx2 = indices[i + 1];
                string prefix = this.Body.Substring(lastIdx, idx1);
                this.AnnotationBodyUnits.Add(prefix);
                string annotationString = this.Body.Substring(idx1, idx2).Trim('`');
                if (this.TokenRange == null) {
                    this.TokenRange = fullText.GetTokenRange(annotationString);
                }

                this.AnnotationBodyUnits.Add(annotationString, linkedText:true);
                lastIdx = idx2;
            }
            string suffix = string.Concat(this.Body.Skip(lastIdx));
            this.AnnotationBodyUnits.Add(suffix);
            this.AnnotationPreviewUnits = new TokenizedAnnotation();

            foreach (var unit in this.AnnotationBodyUnits.Tokens) {
                int curLength = 0;
                int toTake = Math.Min(unit.Val.Length, PREVIEW_LENGTH - curLength);
                string val = unit.Val.Substring(0, toTake);
                this.AnnotationPreviewUnits.Add(val, unit.linkedText);
                curLength += val.Length;
                if (curLength > PREVIEW_LENGTH) {
                    continue;
                }
            }
        }

        public int Ord {
            get {
                if (this.TokenRange == null) {
                    return -1;
                }
                return this.TokenRange.StartIdx;
            }
        }
        public TokenRange TokenRange { get; private set; }
        public string Author { get; private set; }
        public string Body { get; private set; }
        public string PreviewText { get; set; }

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

        public List<string> MyProperty { get; set; }
        public TokenizedAnnotation AnnotationBodyUnits { get; private set; }
        public TokenizedAnnotation AnnotationPreviewUnits { get; private set; }


        internal static AnnotationModel FromDictionary(Dictionary<string, string> dict, DocumentModel doc) {
            return new AnnotationModel(dict["Author"], dict["Body"], doc);
        }
    }
}