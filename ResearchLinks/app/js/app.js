var app = angular.module('researchLinks', ['ui.bootstrap']).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: '/app/partials/projectList.html' }).
            otherwise({ redirectTo: '/' })
    });

    app.run(function($rootScope){
            $rootScope.username = '';
            $rootScope.password = ''
            $rootScope.setUserCredentials = function(username, password){
                $rootScope.username = username;
                $rootScope.password = password;
            }
    });

    