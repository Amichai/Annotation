app.controller('mainCtrl',
    ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
        function loadDocuments() {
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
}]);