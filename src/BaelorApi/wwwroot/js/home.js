/// <reference path="Definitions/jquery.d.ts" />
var ApiStuff = (function () {
    function ApiStuff() {
        this.path = "/api/v0/";
    }
    ApiStuff.prototype.doRequest = function (endpoint) {
        $.getJSON(this.path + endpoint, function (data) {
        }).fail(function (jqxhr, textStatus, error) {
        });
    };
    return ApiStuff;
})();
window.onload = function () {
    var api = new ApiStuff();
};
//# sourceMappingURL=home.js.map