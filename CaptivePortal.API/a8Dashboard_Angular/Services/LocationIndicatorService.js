a8DashboardModule.service('LocationIndicatorService', ['$http', '$rootScope', function ($http, $rootScope) {

    function GetSiteNameFromQueryString() {
        return localStorage.getItem('SiteName').toString().trim();
    }

    function getParameterByName(name, url) {
        var key = null;
        var value = null;
        var retStr = "";
        var queries = {};
        queries = document.location.search.substr(1).split('?');
        if (queries != null) {
            var i = queries.toString().split('=');
            key = i[0].toString();
            value = i[1].toString();
        }
        if (key == "id") {
            retStr = value;
        }
        return retStr;
    }

    this.Index = function () {
        return $http({
            method: "GET",
            url: "/locationIndicators/api/Index?SiteName=" + GetSiteNameFromQueryString(),
            dataType: JSON
        });
    };
    this.editDetail = function () {
        return $http({
            method: "GET",
            url: "/locationIndicators/api/Edit?id=" + getParameterByName() + "&SiteName="+GetSiteNameFromQueryString(),
            dataType: JSON
        });
    };

    this.deleteLocIndicator = function (AreaOfInterestId, LoctionIndicatorId) {
        var objLocation = { SiteName: GetSiteNameFromQueryString(), LoctionIndicatorId: LoctionIndicatorId }
        return $http({
            method: "POST",
            data: objLocation,
            url:"/locationIndicators/api/DeleteLocationIndicator"

        });

    };
    this.deleteNeighArea = function (AreaOfInterestId, NeighBourId) {
        var objNeighBour = { SiteName: GetSiteNameFromQueryString(),NeighBourId: NeighBourId }
        return $http({
            method: "POST",
            data: objNeighBour,
            url: "/locationIndicators/api/DeleteNeighBourArea"
        });
    };

    this.deleteLocation = function (AreaOfInterestId) {
        var objLocation = { SiteName: GetSiteNameFromQueryString(), AreaOfInterestId: AreaOfInterestId }
        return $http({
            method: "POST",
            data: objLocation,
            url:"/locationIndicators/api/Delete"

        });
    }

    this.editLocAndNegh = function (editInfo) {
        editInfo.SiteName = localStorage.getItem('SiteName').toString().trim();
        return $http({
            method: "POST",
            data: editInfo,
            url:"/locationIndicators/api/Edit"
        });
    }

}]);