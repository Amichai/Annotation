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
    public class DynamoDBConnection : Annotation.Web.Data.IDataManager {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAmazonDynamoDB client;

        private DynamoDBConnection(RegionEndpoint endpoint = null) {
            if (endpoint == null) {
                endpoint = RegionEndpoint.USEast1;
            }
            client = new AmazonDynamoDBClient(endpoint);
        }

        public static DynamoDBConnection Instance = new DynamoDBConnection();

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

        public Dictionary<string, string> GetUser(
            string userId
            ) {
                //string indexName = "UserId-index";
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
    }
}