angular.module("projectService", ["ngResource"]).
    factory("Project", function ($resource) {
        return $resource(
            "/api/projects/:Id",
            { Id: "@Id" },
            { "update": { method: "PUT"} }
        );
    });