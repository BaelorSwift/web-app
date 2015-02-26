/// <reference path="Definitions/jquery.d.ts" />
/// <reference path="Definitions/highlightjs.d.ts" />
var HomeVisualStateManager;
(function (HomeVisualStateManager) {
    function loaderVisualModifier(show) {
        var loader = document.getElementsByClassName("loader")[0];
        if (show) {
            loader.style.height = "240px";
            loader.style.opacity = "1.0";
            loader.style.paddingTop = "140px";
        }
        else {
            loader.style.height = "0";
            loader.style.opacity = "0";
            loader.style.paddingTop = "0";
        }
    }
    HomeVisualStateManager.loaderVisualModifier = loaderVisualModifier;
    function apiDemoResponseVisualModifier(show) {
        var apiDemoResponse = document.getElementById("api-demo-response");
        if (show) {
            apiDemoResponse.style.display = "block";
        }
        else {
            apiDemoResponse.style.display = "none";
        }
    }
    HomeVisualStateManager.apiDemoResponseVisualModifier = apiDemoResponseVisualModifier;
})(HomeVisualStateManager || (HomeVisualStateManager = {}));
var JsonHelpers;
(function (JsonHelpers) {
    function formatJsonString(data) {
        return JSON.stringify(JSON.parse(data), null, 4);
    }
    JsonHelpers.formatJsonString = formatJsonString;
})(JsonHelpers || (JsonHelpers = {}));
var ApiStuff = (function () {
    function ApiStuff() {
        this.path = "/api/v0/";
        this.genericError = {
            result: null,
            success: false,
            error: {
                details: null,
                status_code: 0x1069,
                description: "generic_server_error"
            }
        };
    }
    ApiStuff.prototype.doRequest = function (endpoint) {
        HomeVisualStateManager.apiDemoResponseVisualModifier(false);
        HomeVisualStateManager.loaderVisualModifier(true);
        $.getJSON(this.path + endpoint, function (data) {
            document.getElementById("json-preview").textContent = JSON.stringify(data, null, 4).trim();
            document.getElementById("api-demo-response-header").textContent = "api response - http(200)";
        }).fail(function (jqxhr, textStatus, error) {
            document.getElementById("api-demo-response-header").textContent = "api response - http(" + jqxhr.status + ")";
            if (jqxhr.responseText.indexOf("{") == 0) {
                document.getElementById("json-preview").textContent = JsonHelpers.formatJsonString(jqxhr.responseText).trim();
            }
            else {
                document.getElementById("json-preview").textContent = JSON.stringify(this.genericError, null, 4).trim();
            }
        }).always(function () {
            HomeVisualStateManager.loaderVisualModifier(false);
            HomeVisualStateManager.apiDemoResponseVisualModifier(true);
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
