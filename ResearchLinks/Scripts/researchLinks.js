var app = angular.module('researchLinks', ['projectService', 'ui.bootstrap']).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: 'projectList.html' }).
            otherwise({ redirectTo: '/' })
    });

    app.run(function ($rootScope) {
        $rootScope.username = 'james';
        $rootScope.password = 'james2013';
    });

    function ListCtrl($scope, $rootScope, Project) {
        $scope.alerts = [];
        Project.query({}, function (data) {
            $scope.projects = data;
        }, function (response) {
            if (response.status !== 200) {
                $scope.alerts.push({ type: 'error', msg: "Error: " + response.data.message});
            }
    });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
}