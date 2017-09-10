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
    var URL ="/ImportSftpData/api/LoadSftpData?strDateFormat=" + object + "&&ConnectionString=" + GetSiteNameFromQueryString();
    return $http.get(URL);
};


this.GetFileNames = function () {
    return $http({
        method: "GET",
        url: "/ImportSftpData/api/GetFileNames?ConnectionString=" + GetSiteNameFromQueryString(),
        dataType: JSON
    });
}

this.ImportSftpFile = function (data) {
    var URL = "/ImportSftpData/api/ImportCSVFile?lstDataFileIds=" + data + "&&ConnectionString=" + GetSiteNameFromQueryString();
    return $http.get(URL);
}

this.ClearSftpFile = function (data) {
    var URL = "/ImportSftpData/api/ClearFile?lstDataFileIds=" + data + "&&ConnectionString=" + GetSiteNameFromQueryString();
    return $http.get(URL);
}
this.DeleteFile = function (data) {
    return $http({
        method: "GET",
        url: "/ImportSftpData/api/DeleteFileData?Id=" + data + "&&ConnectionString=" + GetSiteNameFromQueryString(),
        dataType: JSON
    });
}

this.SaveSftpPath = function (sftpfilePath) {
    return $http({
        method: "GET",
        url: "/ImportSftpData/api/SaveSftpRemotePath?sftpRemotePath=" + sftpfilePath + "&&ConnectionString=" + GetSiteNameFromQueryString(),
        dataType: JSON
    });
}

this.ImportTUIDisCovery = function (data) {
    data.append("ConnectionString", GetSiteNameFromQueryString())
    return $http.post("/ImportSftpData/api/SaveCruisePlaceDiscovery", data, {
        withCredentials: true,
        headers: { 'Content-Type': undefined },
        transformRequest: angular.identity
    })
}

this.UpdateParameters = function (data, value) {
        return $http.get("/ImportSftpData/api/UpdateParameters?key=" + data + "&&value=" + value + "&&ConnectionString=" + GetSiteNameFromQueryString());
    }
}]);