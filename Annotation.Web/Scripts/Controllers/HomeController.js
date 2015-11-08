app.controller('mainCtrl',
    ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
        function loadDocuments() {
            $scope.newDocumentBody = '';
            $scope.newDocumentTitle = '';
            $scope.newDocumentAuthor = '';
            $http.get(baseUrl + 'api/Document').success(function (docs) {
                $scope.documents = docs;
                $scope.documentsSafe = [].concat($scope.documents);
            });
        }

        loadDocuments();

        $scope.addNewDocument = function () {
            var newDoc = {
                Body: $scope.newDocumentBody,
                Title: $scope.newDocumentTitle,
                Author: $scope.newDocumentAuthor
            }
            $http.post(baseUrl + 'api/Document', newDoc).success(function () {
                loadDocuments();
                $('#newDocumentModal').modal('hide');
            });
        }

        $scope.confirmEdit = function () {
            var newDoc = {
                Body: $scope.newDocumentBody,
                Title: $scope.newDocumentTitle,
                Author: $scope.newDocumentAuthor,
                Id: $scope.editingId,
                Owner: $scope.editingOwner
            }
            $http.put(baseUrl + 'api/Document', newDoc).success(function () {
                loadDocuments();
                $('#editDocumentModal').modal('hide');
                $scope.editingId = '';
                $scope.editingOwner = '';
            });
        }

        $scope.edit = function (row) {
            $http.get(baseUrl + 'api/Document/' + row.Id).success(function (doc) {
                $scope.newDocumentAuthor = row.Author;
                $scope.newDocumentTitle = row.Title;
                $scope.newDocumentBody = doc.Body;
                $scope.editingId = row.Id;
                $scope.editingOwner = row.Owner;
                $('#editDocumentModal').modal('show');
            });
        }

        $scope.confirmDelete = function (doc) {
            $http.delete(baseUrl + 'api/Document/' + $scope.docToDelete.Id).success(function () {
                loadDocuments();
                $('#deleteDocumentModal').modal('hide');
                $scope.docToDelete = '';
            });
        }

        $scope.trash = function (doc) {
            $scope.docToDelete = doc;
            $('#deleteDocumentModal').modal('show');
        }
}]);