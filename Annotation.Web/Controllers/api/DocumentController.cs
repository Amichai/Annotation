using Annotation.Web.Models;
using Annotation.Web.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api
{
    public class DocumentController : ApiController
    {
        public static List<DocumentModel> documents = new List<DocumentModel>();
        public DocumentController() {
            string mockText = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Placeholder.txt"));
            if (documents.Count == 0) {
                documents.Add(new DocumentModel(mockText) {
                    AnnotationCount = 10,
                    Owner = "Test1",
                    Title = "Paradise Lost",
                    Id = Guid.NewGuid()
                });
                documents.Add(new DocumentModel(mockText) {
                    AnnotationCount = 20,
                    Owner = "Test2",
                    Title = "All Things Considered",
                    Id = Guid.NewGuid()
                });
            }
        }
        // GET: api/Document
        public IEnumerable<DocumentInfo> Get() {
            return documents.Select(i => i.Info);
        }

        // GET: api/Document/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Document
        public DocumentModel Post([FromBody]DocumentModel doc) {
            documents.Add(doc);
            doc.Owner = IdentityUtil.GetCurrentUser().UserId;
            return doc;
        }

        // PUT: api/Document/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Document/5
        public void Delete(int id)
        {
        }
    }
}
