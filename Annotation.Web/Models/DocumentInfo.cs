using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class DocumentInfo {
        public string Title { get; set; }
        public string Owner { get; set; }
        public int AnnotationCount { get; set; }
        public string Author { get; set; }
        public Guid Id { get; set; }

        internal static DocumentInfo FromJson(string p) {
            throw new NotImplementedException();
        }

        internal static DocumentInfo FromDictionary(Dictionary<string, string> dict) {
            return new DocumentInfo() {
                Title = dict["Title"],
                Owner = dict["Owner"],
                Author = dict["Author"],
                AnnotationCount = int.Parse(dict["AnnotationCount"]),
                Id = Guid.Parse(dict["DocumentId"])
            };
        }

        private static Random rand = new Random();

        internal static DocumentInfo Random() {
            return new DocumentInfo() {
                Id = Guid.NewGuid(),
                Owner = "test",
                Title = "Title_" + rand.Next(0, 100)
            };
        }
    }
}