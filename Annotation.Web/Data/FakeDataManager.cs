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

        public Guid AddTimeZone(string name, string city, double offset, string userId) {
            if(!this.userTimeZones.ContainsKey(userId)) {
                this.userTimeZones[userId] = new List<TimeZoneModel>();
            }
            var id = Guid.NewGuid();
            this.userTimeZones[userId].Add(new TimeZoneModel() {
                City = city,
                Name = name, 
                GMTOffset = offset,
                Id = id
            });
            return id;
        }

        public void DeleteTimeZone(string userId, Guid id) {
            var tz = this.userTimeZones[userId].Where(i => i.Id == id).Single();
            this.userTimeZones[userId].Remove(tz);
        }

        public List<UserModel> GetAllUsers(int limit) {
            return this.userIdToUser.Values.Select(i => UserModel.FromJson(i)).ToList();
        }

        public TimeZoneModel GetTimeZone(Guid id) {
            return userTimeZones.SelectMany(i => i.Value).Where(i => i.Id == id).SingleOrDefault();
        }

        private Dictionary<string, List<TimeZoneModel>> userTimeZones = new Dictionary<string, List<TimeZoneModel>>();

        public List<TimeZoneModel> GetTimeZones(string userId) {
            if (!this.userTimeZones.ContainsKey(userId)) {
                return new List<TimeZoneModel>();
            }
            return this.userTimeZones[userId];
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