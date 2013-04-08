app.factory("Projects", function($rootScope, $http){
	return {
			 projects : function() {
			     return $http({ method: 'GET', url: "/api/projects", headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $rootScope.password)} })
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