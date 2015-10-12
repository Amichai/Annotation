using Newtonsoft.Json.Linq;
using System;
using Annotation.Web.Models;
namespace Annotation.Web.Data {
    public interface IDataManager {
        void AddNewUser(string userId, JObject value);
        System.Collections.Generic.List<UserModel> GetAllUsers(int limit);
        System.Collections.Generic.Dictionary<string, string> GetUser(string userId);
        Newtonsoft.Json.Linq.JObject RetrieveUser(string userId);
        bool UpdateUser(UserModel user);
    }
}
