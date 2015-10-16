using Annotation.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api {
    public class AnnotationController : ApiController {
        public IEnumerable<AnnotationModel> Get(Guid id) {
            List<AnnotationModel> toReturn = new List<AnnotationModel>();
            for (int i = 0; i < 200; i++) {
                toReturn.Add(new AnnotationModel() {
                    Author = string.Format("Testing{0}", i),
                    Body = string.Format("Body{0}", i),
                });
            }
            return toReturn;
        }

        // POST: api/Annotation
        public void Post([FromBody]string value) {
        }

        // PUT: api/Annotation/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/Annotation/5
        public void Delete(int id) {
        }
    }
}
