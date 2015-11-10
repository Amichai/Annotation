using Newtonsoft.Json.Linq;
using System;
using Annotation.Web.Models;
using System.Collections.Generic;
namespace Annotation.Web.Data {
    public interface IDataManager {
        bool UpdateUser(UserModel user);
        List<UserModel> GetAllUsers(int limit);
        void AddNewUser(string userId, JObject value);
        Dictionary<string, string> GetUser(string userId);
        DocumentModel GetDocument(Guid id);
        IEnumerable<DocumentInfo> GetUserDocuments(string userId);
        bool AddDocument(DocumentModel doc);
        void AddAnnotation(NewAnnotationModel newAnnotation, string userId);
        List<AnnotationModel> GetAnnotations(Guid documentId, string userId, DocumentModel doc);
        void ArchiveDocument(Guid docId);

    }
}
