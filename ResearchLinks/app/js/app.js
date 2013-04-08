﻿var app = angular.module('researchLinks', ['ui.bootstrap']).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: ProjectListCtrl, templateUrl: '/app/partials/projectList.html' }).
            when('/login',{ controller: LoginCtrl, templateUrl: '/app/partials/login.html' }).
            when('/new',{ controller: ProjectNewCtrl, templateUrl: '/app/partials/projectnew.html' }).
            when('/edit/:projectId',{ controller: ProjectEditCtrl, templateUrl: '/app/partials/projectEdit.html' }).
            otherwise({ redirectTo: '/' })
    });

    app.run(function($rootScope){
            $rootScope.username;
            $rootScope.password;
            $rootScope.setUserCredentials = function(username, password){
                $rootScope.username = username;
                $rootScope.password = password;
            }
    });

    