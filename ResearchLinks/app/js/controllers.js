function ProjectListCtrl($scope, $http, $location, Projects) {
    $scope.alerts = [];
    var result = Projects.projects();

    result.success(function (data, status) {
        $scope.projects = data;
    });

    result.error(function (data, status) {
        HandleError(data, status, $scope, $location)
    });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
};

function ProjectNewCtrl($scope, $http, $location, Projects) {
    $scope.alerts = [];

    $scope.save = function () {
      var result = Projects.addProject($scope);

      result.success(function (data, status) {
        $location.path( "/" );
      });

      result.error(function (data, status) {
          HandleError(data, status, $scope, $location);
      });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

};

function ProjectEditCtrl($scope, $http, $location, $routeParams, Projects) {
    $scope.alerts = [];

    var getResult = Projects.getProject($routeParams.projectId);

    getResult.success(function (getData,status) {
      $scope.name = getData.name;
      $scope.description = getData.description;
    });

    getResult.error(function(data,status){
        HandleError(data, status, $scope, $location);
    });
    

    $scope.update = function () {
      var result = Projects.editProject($routeParams.projectId, $scope);

      result.success(function (data, status) {
        $location.path( "/" );
      });

      result.error(function (data, status) {
          HandleError(data, status, $scope, $location);
      });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

};

function ProjectDeleteCtrl($scope, $http, $location, $routeParams, Projects) {
    $scope.alerts = [];

    var getResult = Projects.getProject($routeParams.projectId);

    getResult.success(function (getData,status) {
      $scope.name = getData.name;
      $scope.description = getData.description;
    });

    getResult.error(function(data,status){
        HandleError(data, status, $scope, $location);
    });
    

    $scope.delete = function () {
      var result = Projects.deleteProject($routeParams.projectId, $scope);

      result.success(function (data, status) {
        $location.path( "/" );
      });

      result.error(function (data, status) {
          HandleError(data, status, $scope, $location);
      });
    };

    $scope.cancel = function () {
        $location.path( "/" );
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

};

function ItemListCtrl($scope, $http, $location, $routeParams, ResearchItems) {
    $scope.alerts = [];
    var result = ResearchItems.items($routeParams.projectId);

    result.success(function (data, status) {
        $scope.items = data;
    });

    $scope.projectId = $routeParams.projectId;

    result.error(function (data, status) {
        HandleError(data, status, $scope, $location)
    });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
};

function ItemNewCtrl($scope, $http, $location, $routeParams, ResearchItems) {
    $scope.alerts = [];

    $scope.save = function () {
      var result = ResearchItems.addResearchItem($routeParams.projectId, $scope);

      result.success(function (data, status) {
        $location.path( "/" + data.projectId + "/items");
      });

      result.error(function (data, status) {
          HandleError(data, status, $scope, $location);
      });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

};


function LoginCtrl($scope, $http, $location, Login) {
     // This will normally be called in the login controller.
    $scope.alerts = [];
    
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.login = function(){
      $scope.setUserCredentials($scope.username, $scope.password);
      var result = Login.login();

      result.success(function () {
          $location.path( "/" );
      });

      result.error(function (data, status) {
          HandleError(data, status, $scope, $location);
      });
    };
};

function HandleError(data, status, $scope, $location) {
  if (status == 401 && $location.$$path != "/login"){
      $location.path( "/login" );
  }
  else {
    var message;
    if (data.message) {
        message = data.message;
    } else {
        message = status;
    }
    $scope.alerts.push({ type: 'error', msg: "Error: " + message });
  }
}