function ListCtrl($scope, $http, $location, Projects) {
    $scope.alerts = [];
    var result = Projects.projects();
              result.success(function (data, status) {
                  $scope.projects = data;
              });

              result.error(function (data, status) {
                  if (status == 401){
                      $location.path( "/login" );
                  }
                  else if (status !== 200) {
                      $scope.alerts.push({ type: 'error', msg: "Error: " + data.message });
                  }
              });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
};

function LoginCtrl($scope, $http, $location) {
     // This will normally be called in the login controller.
    $scope.alerts = [];
    
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.login = function(){
      $scope.setUserCredentials($scope.username, $scope.password);
      $http({ method: 'POST', url: "/api/login", headers: { 'Authorization': 'Basic ' + Base64.encode($scope.username + ':' + $scope.password)} }).
              success(function (data, status) {
                  $location.path( "/" );
              }).
              error(function (data, status) {
                  if (status !== 200) {
                      $scope.alerts.push({ type: 'error', msg: "Error: " + data.message });
                  }
              });
    }
};