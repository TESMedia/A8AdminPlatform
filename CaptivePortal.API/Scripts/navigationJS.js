function navigation() {
    var _dashboardUrl = localStorage.getItem('dashboardUrl');
    var _managesite = localStorage.getItem('ManageSite');
    var _rtls = localStorage.getItem('rtlsUrl');
    var _cpt = localStorage.getItem('cptUrl');

    if (_managesite != 'null') {
        document.getElementById('managesite').className = "row margin";
    }
    else {
        document.getElementById('managesite').className = "row margin hide";

    }

    if (_rtls != 'null') {
        document.getElementById('managertls').className = "row margin";
    }
    else {
        document.getElementById('managertls').className = "row margin hide";

    }

    if (_cpt != 'null' && _dashboardUrl != 'null') {
        document.getElementById('managewifiuser').className = "row margin";
        document.getElementById('viewdashboard').className = "row margin";
        document.getElementById('wifisummary').className = "unhide";
        document.getElementById('locationdashboard').className = "unhide";
        document.getElementById('NavPromotional').className = 'unhide';
        document.getElementById('managedashboard').className = "row margin";
    }
    else if (_cpt != 'null') {
        document.getElementById('managewifiuser').className = "row margin";
        document.getElementById('viewdashboard').className = "row margin";
        document.getElementById('wifisummary').className = "unhide";
        document.getElementById('locationdashboard').className = "hide";
        document.getElementById('managedashboard').className = "row margin hide";
        document.getElementById('NavPromotional').className = 'unhide';
    }
    else if (_dashboardUrl != 'null') {
        document.getElementById('viewdashboard').className = "row margin";
        document.getElementById('managedashboard').className = "row margin";
        document.getElementById('locationdashboard').className = "unhide";
        document.getElementById('wifisummary').className = "hide";
        document.getElementById('managewifiuser').className = "hide";
    }

}