using Annotation.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api {
    public class AnnotationController : ApiController {
        private static Random rand = new Random();
        private static string mockText = null;
        public static List<AnnotationModel> RandomAnnotations() {
            if (mockText == null) {
                mockText = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Placeholder2.txt"));
            }
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
                string annotationText = getRandomAnnotation();
                toReturn.Add(new AnnotationModel(string.Format("Testing{0}", i), "`" + quote + "` " + annotationText, doc));
            }
            return toReturn;
        }

        private const int maxAnnotationLength = 2000;

        private static string getRandomAnnotation() {
            int charCount = mockText.Length;
            int startIdx = rand.Next(0, charCount - maxAnnotationLength);
            int toTake = rand.Next(10, maxAnnotationLength);
            return mockText.Substring(startIdx, toTake);
        }
    }
}
