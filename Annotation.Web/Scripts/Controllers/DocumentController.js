app.controller('documentCtrl',
    ['$scope', '$http', function ($scope, $http) {
        //$http.get(baseUrl + 'api/Annotation/' + documentId).success(function (annotations) {
        //    ///Remove this api call in favor of the annotated document
        //    $scope.annotations = annotations;
        //});

        $http.get(baseUrl + 'api/AnnotatedDocument/' + documentId).success(function (annotatedDocument) {
            $scope.annotations = annotatedDocument.Annotations;
            $scope.document = annotatedDocument.Document;
        });

        $scope.pageHeight = window.innerHeight - 60;

        $scope.hoverToken = function (token, tokenIndex, entered) {
            for(var i=0; i < token.LinkedAnnotations.length; i++) {
                var annotationIndex = token.LinkedAnnotations[i];
                if (entered) {
                    $('#annotation_' + annotationIndex).addClass('highlightedText');
                    $('#token_' + tokenIndex).addClass('highlightedText');
                } else {
                    $('#annotation_' + annotationIndex).removeClass('highlightedText');
                    $('#token_' + tokenIndex).removeClass('highlightedText');
                }
            }
        }

        $scope.expandAnnotation = function (annotation) {
            var id = annotation.TokenRange.StartIdx;
            document.getElementById('token_' + id).scrollIntoView(true);
        }

        $scope.clickToken = function (token) {
            var id = token.LinkedAnnotations[0];
            document.getElementById('annotation_' + id).scrollIntoView(true);
        }

        $scope.hover = function (ann, annotationIndex, entered) {
            var start = ann.TokenRange.StartIdx;
            var range = ann.TokenRange.Range;
            if (entered) {
                for (var idx = start; idx < start + range; idx++) {
                    $('#token_' + idx).addClass('highlightedText');
                }
                $('#annotation_' + annotationIndex).addClass('highlightedText');
            } else {
                for (var idx = start; idx < start + range; idx++) {
                    $('#token_' + idx).removeClass('highlightedText');
                }
                $('#annotation_' + annotationIndex).removeClass('highlightedText');
            }
        }
            
        window.addEventListener('resize', onResize, false);
        function onResize() {
            $scope.pageHeight = window.innerHeight - 60;
            $scope.$apply();
        }
    }]);