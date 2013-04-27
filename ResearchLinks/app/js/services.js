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
			return $http({ method: 'PUT', url: "/api/projects/" + projectId.toString(), data: { name: $scope.name, description: $scope.description, userName: $rootScope.username}, headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		deleteProject : function(projectId, $scope) {
			return $http({ method: 'DELETE', url: "/api/projects/" + projectId.toString(), headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		}
	}

});

app.factory("ResearchItems", function($rootScope, $http){
	return {
		items : function(projectId) {
		    return $http({ method: 'GET', url: "/api/projects/" + projectId + "/researchItems", headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		addResearchItem : function(projectId, $scope) {
			return $http({ method: 'POST', url: "/api/projects/" + projectId + "/researchItems", data: {subject: $scope.subject, description: $scope.description, userName: $rootScope.username}, headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		getResearchItem : function(projectId, researchItemId) {
			return $http({ method: 'GET', url: "/api/projects/" + projectId.toString() + "/researchItems/" + researchItemId.toString(), headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		editResearchItem: function(projectId, researchItemId, $scope) {
			return $http({ method: 'PUT', url: "/api/projects/" + projectId.toString() + "/researchItems/" + researchItemId.toString(), data: { subject: $scope.subject, description: $scope.description, userName: $rootScope.username}, headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
		},

		deleteResearchItem: function(projectId, researchItemId, $scope) {
			return $http({ method: 'DELETE', url: "/api/projects/" + projectId.toString() + "/researchItems/" + researchItemId.toString(), headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
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

