function navigation() {

    var _managedashboard = localStorage.getItem('ManageDashboard');
    var _managesite = localStorage.getItem('ManageSite');
    var _rtls = localStorage.getItem('RTLS');
    var _viewdashboard = localStorage.getItem('ManageDashboard');
    var _managewifiuser = localStorage.getItem('ViewDashboard');

    if (_managewifiuser != 'null') {
        document.getElementById('wifisummary').className = "unhide";
    }
    else {
        document.getElementById('wifisummary').className = "hide";

    }
    if (_managedashboard != 'null') {
        document.getElementById('managedashboard').className = "row margin";
    }
    else {
        document.getElementById('managedashboard').className = "row margin hide";

    }
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
    if (_viewdashboard != 'null') {
        document.getElementById('viewdashboard').className = "row margin";
    }
    else {
        document.getElementById('viewdashboard').className = "row margin hide";

    }

    if (_managewifiuser != 'null') {
        document.getElementById('managewifiuser').className = "row margin";
    }
    else {
        document.getElementById('managewifiuser').className = "row margin hide";

    }
}