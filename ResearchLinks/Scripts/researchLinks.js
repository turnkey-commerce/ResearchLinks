angular.module('researchLinks', ['projectService', 'ui.bootstrap'])
    .config(function ($routeProvider) {
    $routeProvider.
      when('/', { controller: ListCtrl, templateUrl: 'projectList.html' }).
      otherwise({ redirectTo: '/' });
});

function ListCtrl($scope, Project) {
    $scope.alerts = [];
    Project.query({}, function (data) {
        $scope.projects = data;
    }, function (response) {
        if (response.status !== 200) {
            $scope.alerts.push({ type: 'error', msg: "Error: " + response.message});
        }
    });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
}