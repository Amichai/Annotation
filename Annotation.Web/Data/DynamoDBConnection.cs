using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Annotation.Web.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Annotation.Web.Data {
    public class DynamoDBConnection : IDataManager {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAmazonDynamoDB client;

        private DynamoDBConnection(RegionEndpoint endpoint = null) {
            if (endpoint == null) {
                endpoint = RegionEndpoint.USEast1;
            }
            client = new AmazonDynamoDBClient(endpoint);
        }

        public static IDataManager Instance = new DynamoDBConnection();
        //public static IDataManager Instance = new FakeDataManager();

        public JObject RetrieveUser(string userId) {
            throw new Exception();
        }

        private List<Dictionary<string, string>> get(
            string table,
            string keyName,
            string keyValue) {
            string prefixedKeyName = ":v_" + keyName;
            AttributeValue av = new AttributeValue { S = keyValue };
            var response = this.client.Query(new QueryRequest(table) {
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    { prefixedKeyName, av}
                },
                KeyConditionExpression = string.Format("{0} = {1}", keyName, prefixedKeyName),
            });
            if (response.Items.Count == 0) {
                return new List<Dictionary<string,string>>();
            }
            log.InfoFormat("Queried: {0}, {1}, {2}", table, keyName, keyValue);
            return response.Items.Select(i => i.ToDictionary(j => j.Key, j => j.Value.S)).ToList();
        }

        private const string USER_TABLE = @"annotation_users";
        private const string USER_DOCUMENTS = @"annotation_userDocuments";
        private const string DOCUMENTS_TABLE = @"annotation_documents";
        private const string DOCUMENT_INFO_TABLE = @"annotation_documentInfo";
        private const string ANNOTATION_TABLE = @"annotation_annotations";
        private const string USER_ANNOTATIONS_TABLE = @"annotation_userAnnotations";
        private const string DOCUMENT_ANNOTATIONS_TABLE = @"annotation_documentAnnotations";

        public Dictionary<string, string> GetUser(
            string userId
            ) {
                string indexName = "UserId";
                string propertyName = "UserId";
                var matches = this.get(USER_TABLE, propertyName, userId);
                log.InfoFormat("Queried {0}, {1}, {2}, {3}", USER_TABLE, indexName, propertyName, userId);
                var match = matches.SingleOrDefault();
                return match;
        }

        public void AddNewUser(string userId, JObject value) {
            this.add(USER_TABLE, "UserId", userId, new TableAttribute("UserInfo", value.ToString()));
        }

        private DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private enum ValueType { S, N };
        private struct TableAttribute {
            public TableAttribute(string name, string value, ValueType type = ValueType.S)
                : this() {
                this.AttributeName = name;
                this.AttributeValue = value;
                this.AttributeType = type;
            }
            public string AttributeValue { get; private set; }
            public string AttributeName { get; private set; }
            public ValueType AttributeType { get; set; }
            public AttributeValue ToAttributeValue() {
                switch (this.AttributeType) {
                    case ValueType.S:
                        return new AttributeValue { S = this.AttributeValue };
                    case ValueType.N:
                        return new AttributeValue { N = this.AttributeValue };
                    default:
                        throw new Exception("Unknown type");
                }
            }
        }

        private void add(string table, string keyName, string key, params TableAttribute[] values) {
            Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>();
            attributes[keyName] = new AttributeValue { S = key };
            foreach (var v in values) {
                attributes[v.AttributeName] = new AttributeValue { S = v.AttributeValue };
            }
            var response = client.BatchWriteItem(new BatchWriteItemRequest() {
                RequestItems = new Dictionary<string, List<WriteRequest>>() {
                    {  
                      table,
                      new List<WriteRequest>() {
                           new WriteRequest(
                               new PutRequest(
                                   attributes
                                   )
                                )
                            }
                        }
                    },
            }
            );
            Debug.Assert(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Debug.Assert(response.UnprocessedItems.Count == 0);
            log.Info(string.Format("Wrote Table: {0}, key: {1}, values: {2}", table, key, string.Join(",", values.Select(i => i.AttributeValue))));
        }


        private void delete(string table, string keyName, string keyValue) {
            this.client.DeleteItem(table,
                new Dictionary<string, AttributeValue>() {
                    { 
                        keyName, new AttributeValue { S = keyValue }
                    }
                }
            );
            log.InfoFormat("Delete: {0}, {1}, {2}", table, keyValue, keyName);
        }

        private List<Dictionary<string, string>> scan(string table, int limit) {
            var response = this.client.Scan(new ScanRequest(table) {
                Limit = limit,
            });
            var toReturn = response.Items.Select(i => i.ToDictionary(j => j.Key, j => j.Value.S)).ToList();
            log.InfoFormat("Scan got {0} from: {1}", toReturn.Count, table);
            return toReturn;
        }

        public List<UserModel> GetAllUsers(int limit) {
            return this.scan(USER_TABLE, limit).Select(i => {
                var info = JObject.Parse(i["UserInfo"]);
                return new UserModel() {
                    FirstName = info["FirstName"].Value<string>(),
                    LastName = info["LastName"].Value<string>(),
                    Role = info["Role"].Value<string>(),
                    UserId = info["UserId"].Value<string>()
                };
            }).ToList();
        }

        public bool UpdateUser(UserModel user) {
            try {
                var match = this.GetUser(user.UserId);
                user.Password = JObject.Parse(match["UserInfo"])["Password"].Value<string>();
                this.add(USER_TABLE, "UserId", user.UserId, new TableAttribute("UserInfo", user.ToJson().ToString()));
                return true;
            } catch (Exception ex) {
                log.Error("Update user failed", ex);
                return false;
            }
        }

        public DocumentInfo GetDocumentInfo(Guid id) {
            var doc = this.get(DOCUMENT_INFO_TABLE, "DocumentId", id.ToString());
            return DocumentInfo.FromDictionary(doc[0]);
        }

        public IEnumerable<DocumentInfo> GetUserDocuments(string userId) {
            var a = this.get(USER_DOCUMENTS, "UserId", userId);
            var ids = JArray.Parse(a[0]["DocumentIds"]);
            foreach (var id in ids) {
                yield return this.GetDocumentInfo(Guid.Parse(id.Value<string>()));
            }
           
        }

        public DocumentModel GetDocument(Guid id) {
            var doc = this.get(DOCUMENTS_TABLE, "DocumentId", id.ToString())[0];
            var toReturn = DocumentModel.FromDictionary(doc);
            var info = this.GetDocumentInfo(id);
            toReturn.Info = info;
            return toReturn;
        }

        public bool AddDocument(DocumentModel doc) {
            this.add(DOCUMENTS_TABLE, "DocumentId", doc.Id.ToString(),
                new TableAttribute("Body", doc.Body));
            this.add(DOCUMENT_INFO_TABLE, "DocumentId", doc.Id.ToString(),
                new TableAttribute("Title", doc.Title),
                new TableAttribute("Owner", doc.Owner),
                new TableAttribute("AnnotationCount", doc.AnnotationCount.ToString()),
                new TableAttribute("Id", doc.Id.ToString()),
                new TableAttribute("Author", doc.Author),
                new TableAttribute("IsArchived", doc.IsArchived.ToString())
                );
            var a = this.get(USER_DOCUMENTS, "UserId", doc.Owner.ToString());
            var ids = JArray.Parse(a[0]["DocumentIds"]);
            if (ids.Any(i => i.ToString() == doc.Id.ToString())) {
                return true;
            }
            ids.Add(doc.Id);
            
            this.add(USER_DOCUMENTS, "UserId", doc.Owner,
                new TableAttribute("DocumentIds", ids.ToString()));
            return true;
        }

        public void AddAnnotation(Guid id, long ticks, string body, string author) {
            this.add(ANNOTATION_TABLE, "AnnotationId", id.ToString(),
                new TableAttribute("Timestamp", ticks.ToString(), ValueType.N),
                new TableAttribute("Body", body),
                new TableAttribute("Author", author));
        }

        public void AddAnnotationAndLinkToUser(NewAnnotationModel newAnnotation, string userId) {
            this.AddAnnotation(newAnnotation.AnnotationId, 
                DateTime.Now.Ticks,
                newAnnotation.Body, userId);
            var a = this.get(USER_ANNOTATIONS_TABLE, "UserId", userId);
            JArray ids;
            if (a.Count == 0) {
                ids = JArray.Parse("[]");
            } else {
                ids = JArray.Parse(a[0]["AnnotationIds"]);
            }
            ids.Add(newAnnotation.AnnotationId);
            this.add(USER_ANNOTATIONS_TABLE, "UserId", userId,
                new TableAttribute("AnnotationIds", ids.ToString()));

            string documentId = newAnnotation.DocumentId.ToString();
            a = this.get(DOCUMENT_ANNOTATIONS_TABLE, "DocumentId", documentId);
            if (a.Count == 0) {
                ids = JArray.Parse("[]");
            } else {
                ids = JArray.Parse(a[0]["AnnotationIds"]);
            }
            ids.Add(newAnnotation.AnnotationId);
            this.add(DOCUMENT_ANNOTATIONS_TABLE, "DocumentId", documentId,
                new TableAttribute("AnnotationIds", ids.ToString()));
        }

        public List<AnnotationModel> GetAnnotations(Guid documentId, string userId, DocumentModel doc) {
            //TODO: check the permissions on this user
            var a = this.get(DOCUMENT_ANNOTATIONS_TABLE, "DocumentId", documentId.ToString());
            if (a.Count == 0) {
                return new List<AnnotationModel>();
            }
            var ids = JArray.Parse(a[0]["AnnotationIds"]);
            var annotations = ids.Select(i => this.get(ANNOTATION_TABLE, "AnnotationId", i.ToString()));
            return annotations.Select(i => AnnotationModel.FromDictionary(i[0], doc)).ToList();
        }

        public void ArchiveDocument(Guid docId) {
            var doc = this.GetDocument(docId);
            doc.IsArchived = true;
            this.AddDocument(doc);
        }

        public void DeleteDocument(Guid docId, string userId) {
            var a = this.get(USER_DOCUMENTS, "UserId", userId);
            var ids = JArray.Parse(a[0]["DocumentIds"]);
            var matches = ids.Where(i => i.ToString() == docId.ToString()).ToList();
            foreach (var match in matches) {
                ids.Remove(match);
            }
            this.add(USER_DOCUMENTS, "UserId", userId,
                new TableAttribute("DocumentIds", ids.ToString()));
            this.delete(DOCUMENTS_TABLE, "DocumentId", docId.ToString());
        }

        public void UpdateAnnotation(UpdateAnnotationModel model) {
            var id = model.Id;
            var annotationDict = this.get(ANNOTATION_TABLE, "AnnotationId", id.ToString())[0];
            var ticks = long.Parse(annotationDict["Timestamp"]);
            string body = model.Body;
            string author = annotationDict["Author"];
            this.AddAnnotation(id, ticks, body, author);
        }
    }
}