using Annotation.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api {
    public class AnnotationController : ApiController {
        private static Random rand = new Random();
        public static List<AnnotationModel> RandomAnnotations() {
            List<AnnotationModel> toReturn = new List<AnnotationModel>();
            DocumentModel doc = DocumentController.documents[0];
            var tokens = doc.Tokens;
            int maxLength = 25;
            for (int i = 0; i < 200; i++) {
                int startIdx = rand.Next(0, tokens.Count - maxLength);
                int toTake = rand.Next(2, maxLength);
                string quote = string.Empty;
                for (int j = startIdx; j < startIdx + toTake; j++) {
                    quote += tokens[j].ToString();
                }
                toReturn.Add(new AnnotationModel(string.Format("Testing{0}", i), "`" + quote + "` annotation", doc));
            }
            return toReturn;
        }
    }
}
