app.controller('mainCtrl',
    ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
        $http.get(baseUrl + 'api/Document').success(function (docs) {
            $scope.documents = docs;
            $scope.documentsSafe = [].concat($scope.documents);
        });

        $scope.addNew = function () {
            document.location.href = '/AddNew'
        }
}]);