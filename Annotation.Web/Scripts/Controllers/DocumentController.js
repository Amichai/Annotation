app.controller('documentCtrl',
    ['$scope', '$http', function ($scope, $http) {
        $http.get(baseUrl + 'api/Annotation/' + documentId).success(function (annotations) {
            $scope.annotations = annotations;
        });

        $scope.pageHeight = window.innerHeight - 60;

        window.addEventListener('resize', onResize, false);
        function onResize() {
            $scope.pageHeight = window.innerHeight - 60;
            $scope.$apply();
        }
    }]);