a8DashboardModule.service('UploadFileService', ['$http', '$rootScope', function ($http, $rootScope) {

function GetSiteNameFromQueryString() {
        var queries = {};
        queries = document.location.search.substr(1).split('&');
        var i = queries.toString().split('=');
        key = i[0].toString();
        value = i[1].toString();
        return value;
    }


this.filedownload = function (object) {
    var URL ="/ImportSftpData/LoadSftpData?strDateFormat=" + object + "&&ConnectionString=" + GetSiteNameFromQueryString();
    return $http.get(URL);
};


this.GetFileNames = function () {
    return $http({
        method: "GET",
        url: "/ImportSftpData/GetFileNames?ConnectionString=" + GetSiteNameFromQueryString(),
        dataType: JSON
    });
}

this.ImportSftpFile = function (data) {
    var URL = "/ImportSftpData/ImportCSVFile?lstDataFileIds=" + data + "&&ConnectionString=" + GetSiteNameFromQueryString();
    return $http.get(URL);
}

this.ClearSftpFile = function (data) {
    var URL = "/ImportSftpData/ClearFile?lstDataFileIds=" + data + "&&ConnectionString=" + GetSiteNameFromQueryString();
    return $http.get(URL);
}
this.DeleteFile = function (data) {
    return $http({
        method: "GET",
        url: "/ImportSftpData/DeleteFileData?Id=" + data + "&&ConnectionString=" + GetSiteNameFromQueryString(),
        dataType: JSON
    });
}

this.SaveSftpPath = function (sftpfilePath) {
    return $http({
        method: "GET",
        url: "/ImportSftpData/SaveSftpRemotePath?sftpRemotePath=" + sftpfilePath + "&&ConnectionString=" + GetSiteNameFromQueryString(),
        dataType: JSON
    });
}

this.ImportTUIDisCovery = function (data) {
    data.append("ConnectionString", GetSiteNameFromQueryString())
    return $http.post("api/ImportSftpData/SaveCruisePlaceDiscovery", data, {
        withCredentials: true,
        headers: { 'Content-Type': undefined },
        transformRequest: angular.identity
    })
}

this.UpdateParameters = function (data, value) {
        return $http.get("api/ImportSftpData/UpdateParameters?key=" + data + "&&value=" + value + "&&ConnectionString=" + GetSiteNameFromQueryString());
    }
}]);