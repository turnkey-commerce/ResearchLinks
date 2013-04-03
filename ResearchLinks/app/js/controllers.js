function ListCtrl($scope, $http) {
    $scope.setUserCredentials('james', 'james2013'); // This will normally be called in the login controller.
    $scope.alerts = [];
    $http({ method: 'GET', url: "/api/projects", headers: { 'Authorization': 'Basic ' + Base64.encode($scope.username + ':' + $scope.password)} }).
              success(function (data, status) {
                  $scope.projects = data;
              }).
              error(function (data, status) {
                  if (status !== 200) {
                      $scope.alerts.push({ type: 'error', msg: "Error: " + data.message });
                  }
              });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
};