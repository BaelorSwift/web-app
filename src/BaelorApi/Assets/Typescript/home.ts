/// <reference path="Definitions/jquery.d.ts" />
/// <reference path="Definitions/highlightjs.d.ts" />

module HomeVisualStateManager {
	export function loaderVisualModifier(show: boolean) {
		var loader = <HTMLElement> document.getElementsByClassName("loader")[0];

		if (show) {
			loader.style.height = "240px";
			loader.style.opacity = "1.0";
			loader.style.paddingTop = "140px";
		} else {
			loader.style.height = "0";
			loader.style.opacity = "0";
			loader.style.paddingTop = "0";
		}
	}

	export function apiDemoResponseVisualModifier(show: boolean) {
		var apiDemoResponse = <HTMLElement> document.getElementById("api-demo-response");

		if (show) {
			apiDemoResponse.style.display = "block";
		} else {
			apiDemoResponse.style.display = "none";
		}
	}
}

module JsonHelpers {
	export function formatJsonString(data: string) {
		return JSON.stringify(JSON.parse(data), null, 4);
	}
}

class ApiStuff {
	path: string = "/api/v0/";
	genericError: Object = {
		result: null,
		success: false,
		error: {
			details: null,
			status_code: 0x1069,
			description: "generic_server_error"
		}
	};

	constructor() { }
	
	doRequest(endpoint: string) {
		HomeVisualStateManager.apiDemoResponseVisualModifier(false);
		HomeVisualStateManager.loaderVisualModifier(true);

		$.getJSON(this.path + endpoint, function (data) {
			document.getElementById("json-preview").textContent = JSON.stringify(data, null, 4).trim();
			document.getElementById("api-demo-response-header").textContent = "api response - http(200)";
		})
		.fail(function (jqxhr, textStatus, error) {
			document.getElementById("api-demo-response-header").textContent = "api response - http(" + jqxhr.status + ")";
			if ((<string> jqxhr.responseText).indexOf("{") == 0) {
				document.getElementById("json-preview").textContent = JsonHelpers.formatJsonString(<string> jqxhr.responseText).trim();
			} else {
				document.getElementById("json-preview").textContent = JSON.stringify(this.genericError, null, 4).trim();
			}
		})
		.always(function () {
			HomeVisualStateManager.loaderVisualModifier(false);
			HomeVisualStateManager.apiDemoResponseVisualModifier(true);

			document.getElementById("api-demo-response").style.display = "block";
			hljs.highlightBlock($('pre code')[0]);
		});
	}
}

window.onload = () => {
	var api = new ApiStuff();
	
	document.getElementById("desktop-get-api-preview").onclick = () => {
		api.doRequest((<HTMLInputElement> document.getElementById("desktop-api-preview-path")).value);
	};
	document.getElementById("mobile-get-api-preview").onclick = () => {
		api.doRequest((<HTMLInputElement> document.getElementById("mobile-api-preview-path")).value);
	};
};
