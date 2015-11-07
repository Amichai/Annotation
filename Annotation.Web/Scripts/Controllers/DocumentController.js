app.controller('documentCtrl',
    ['$scope', '$http', function ($scope, $http) {

        function loadAnnotatedDocument() {
            $http.get(baseUrl + 'api/AnnotatedDocument/' + documentId).success(function (annotatedDocument) {
                $scope.annotations = annotatedDocument.Annotations;
                $scope.document = annotatedDocument.Document;
                for (var i = 0; i < $scope.annotations.length; i++) {
                    $scope.annotations[i].isExpanded = false;
                }

                function f() {
                    for (var i = 0; i < $scope.document.Tokens.length; i++) {
                        var token = $scope.document.Tokens[i];
                        var linked = token.LinkedAnnotations;
                        if (linked.length > 0) {
                            $('#token_' + i).addClass('hasAnnotation');
                        }
                    }
                }
                delay(f);
            });
        }
        
        loadAnnotatedDocument();

        $scope.saveAnnotation = function () {
            var newAnnotation = new Object();
            newAnnotation.Body = $scope.newAnnotationText;
            newAnnotation.DocumentId = documentId;
            $http.post(baseUrl + 'api/Annotation/', newAnnotation).success(function (annotatedDocument) {
                $('#newAnnotationModal').modal('hide');
                $scope.newAnnotationText = "";
                loadAnnotatedDocument();
            });
        }

        function delay(callback) {
            setTimeout(callback, 0);
        }

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

        $scope.expandAnnotation = function (annotation, index) {
            if (annotation.TokenRange) {
                var id = annotation.TokenRange.StartIdx;
                document.getElementById('token_' + id).scrollIntoView(true);
            }

            if (annotation.isExpanded) {
                annotation.isExpanded = false;
                return;
            }
            //Get some jslinq here
            for (var i = 0; i < $scope.annotations.length; i++) {
                $scope.annotations[i].isExpanded = false;
            }
            annotation.isExpanded = true;
            setTimeout(function () {
                document.getElementById('annotation_' + index).scrollIntoView(true);
            }, 0);
        }

        $scope.clickToken = function (token) {
            if (token.LinkedAnnotations.length == 0) {
                return;
            }
            var id = token.LinkedAnnotations[0];
            document.getElementById('annotation_' + id).scrollIntoView(true);
        }

        $scope.hover = function (ann, annotationIndex, entered) {
            if (!ann.TokenRange) {
                return;
            }
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