app.controller('documentCtrl',
    ['$scope', '$http', function ($scope, $http) {
        $http.get(baseUrl + 'api/Annotation/' + documentId).success(function (annotations) {
            $scope.annotations = annotations;
        });

        $scope.pageHeight = window.innerHeight - 60;

        $scope.hover = function (unit, entered) {
            var start = unit.TokenRange.StartIdx;
            var range = unit.TokenRange.Range;
            if (entered) {
                for (var idx = start; idx < start + range; idx++) {
                    $('#token_' + idx).addClass('highlightedText');
                }
            } else {
                for (var idx = start; idx < start + range; idx++) {
                    $('#token_' + idx).removeClass('highlightedText');
                }
            }
        }

        $scope.enter = function (unit) {
        }
            
        window.addEventListener('resize', onResize, false);
        function onResize() {
            $scope.pageHeight = window.innerHeight - 60;
            $scope.$apply();
        }
    }]);