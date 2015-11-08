﻿using Annotation.Web.Data;
using Annotation.Web.Models;
using Annotation.Web.Util;
using Newtonsoft.Json.Linq;
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
        // GET: api/Document
        public IEnumerable<DocumentInfo> Get() {
            var currentUser = IdentityUtil.GetCurrentUser();
            IEnumerable<DocumentInfo> documents = DynamoDBConnection.Instance.GetUserDocuments(currentUser.UserId);
            //return documents.se;
            return documents;
        }

        public DocumentModel Get(Guid id) {
            //TODO: Check user permissions
            return DynamoDBConnection.Instance.GetDocument(id);
        }

        // POST: api/Document
        public DocumentModel Post([FromBody]DocumentModel doc) {
            doc.Owner = IdentityUtil.GetCurrentUser().UserId;
            doc.Id = Guid.NewGuid();
            bool success = DynamoDBConnection.Instance.AddDocument(doc);
            return doc;
        }

        // PUT: api/Document/5
        public void Put([FromBody]DocumentModel doc)
        {
            DynamoDBConnection.Instance.AddDocument(doc);
        }

        // DELETE: api/Document/5
        public void Delete(Guid id) {
            var currentUser = IdentityUtil.GetCurrentUser();
            DynamoDBConnection.Instance.DeleteDocument(id, currentUser.UserId);
        }
    }
}
