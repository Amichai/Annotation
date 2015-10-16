using Annotation.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Data {
    public class FakeDataManager : IDataManager {

        private Dictionary<string, JObject> userIdToUser = new Dictionary<string, JObject>() {
            { "test", JObject.Parse(@"{UserInfo: {
FirstName:"""", LastName: """", UserId: ""test"", Role: ""user""
} }")}
        };

        public void AddNewUser(string userId, JObject value) {
            this.userIdToUser[userId] = value;
        }

        public List<UserModel> GetAllUsers(int limit) {
            return this.userIdToUser.Values.Select(i => UserModel.FromJson(i)).ToList();
        }

        public Dictionary<string, string> GetUser(string userId) {
            var obj = this.userIdToUser[userId];
            var toReturn = new Dictionary<string, string>();
            foreach (var p in obj.Properties()) {
                toReturn[p.Name] = p.Value.ToString();
            }
            return toReturn;
        }

        public string GetUserId(string sessionId) {
            throw new NotImplementedException();
        }

        public JObject RetrieveUser(string userId) {
            throw new NotImplementedException();
        }

        public bool UpdateUser(UserModel user) {
            throw new NotImplementedException();
        }
    }
}