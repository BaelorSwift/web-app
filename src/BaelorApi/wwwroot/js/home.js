/// <reference path="Definitions/jquery.d.ts" />
/// <reference path="Definitions\highlightjs.d.ts" />
var ApiStuff = (function () {
    function ApiStuff() {
        this.path = "/api/v0/";
    }
    ApiStuff.prototype.doRequest = function (endpoint) {
        $.getJSON(this.path + endpoint, function (data) {
            document.getElementById("json-preview").innerText = JSON.stringify(data, null, 4).trim();
        }).fail(function (jqxhr, textStatus, error) {
            console.error(textStatus);
        }).always(function () {
            document.getElementById("api-demo-response").style.display = "block";
            hljs.highlightBlock($('pre code')[0]);
        });
    };
    return ApiStuff;
})();
window.onload = function () {
    var api = new ApiStuff();
    document.getElementById("get-api-preview").onclick = function () {
        api.doRequest(document.getElementById("api-preview-path").value);
    };
};
//# sourceMappingURL=home.js.map