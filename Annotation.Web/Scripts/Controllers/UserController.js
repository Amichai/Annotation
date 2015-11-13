app.controller('userCtrl', ['$scope', '$http', function ($scope, $http) {
    
    function loadProfile() {
        $http.get(baseUrl + 'api/User/').success(function (profile) {
            $scope.profile = profile;
        })
    }


}]);