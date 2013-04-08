app.factory("Projects", function($rootScope, $http){
	return {
		projects : function() {
		    return $http({ method: 'GET', url: "/api/projects", headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		addProject : function($scope) {
			return $http({ method: 'POST', url: "/api/projects", data: {name: $scope.name, description: $scope.description, userName: $rootScope.username}, headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		getProject : function(projectId) {
			return $http({ method: 'GET', url: "/api/projects/" + projectId.toString(), headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		editProject : function(projectId, $scope) {
			return $http({ method: 'PUT', url: "/api/projects", data: {projectId: projectId, name: $scope.name, description: $scope.description, userName: $rootScope.username}, headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		}
	}

});

app.factory("Login", function($rootScope, $http){
	return {
		login : function() {
		     return $http({ method: 'POST', url: "/api/login", headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		}
	}

});

