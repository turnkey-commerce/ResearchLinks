app.factory("Projects", function($rootScope, $http){
	return {
			 projects : function() {
			 	return $http({ method: 'GET', url: "/api/projects", headers: { 'Authorization': 'Basic ' + Base64.encode($rootScope.username + ':' + $scope.password)} })
		}
	}

});