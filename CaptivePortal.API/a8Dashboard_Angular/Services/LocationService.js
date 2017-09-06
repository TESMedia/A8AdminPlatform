a8DashboardModule.service('LocationService', ['$http', '$rootScope', function ($http, $rootScope) {

  function GetSiteNameFromQueryString() {
        var queries = {};
        queries = document.location.search.substr(1).split('&');
        var i = queries.toString().split('=');
        key = i[0].toString();
        value = i[1].toString();
        return value;
    }

    this.getLocations = function () {
        return $http({
            method: "GET",
            url: "/DashBoard/api/GetLocation?ConnectionString=" + GetSiteNameFromQueryString(),
            dataType: JSON
        });
    };

    
    this.DateDownload = function () {
        return $http({
            method: "GET",
            url: "/DashBoard/api/GetDate?ConnectionString=" + GetSiteNameFromQueryString(),
            dataType: JSON
        });
    };

    this.GenerateReport = function (searchObject) {
        var URL = "/DashBoard/api/GenerateReport";
        return $http.post(URL, searchObject)
    };

    this.GetListOfSiteWithLocationDashBoard = function (searchObject)
    {
        return $http({
            method: "GET",
            url: "/DashBoard/api/GetListOfSiteWithLocationDashBoard?SiteId=" + searchObject,
            dataType: JSON
        });
    }
   
    //this.ImportTUIDisCovery = function (objFormData) {
    //    alert(objFormData['formData']);
    //    var URL = "api/SaveTUIDiscovery";
    //    return $http.post(URL, objFormData)
    //}    
}]);