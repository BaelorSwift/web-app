/// <reference path="Definitions/jquery.d.ts" />
/// <reference path="Definitions/highlightjs.d.ts" />
/// <reference path="Definitions/gosquared.d.ts" />

module HomeVisualStateManager {
	export function loaderVisualModifier(show: boolean) {
		var loader = <HTMLElement> document.getElementsByClassName("loader")[0];

		if (show) {
			loader.className += " active";
		} else {
			loader.className = loader.className.replace(/(?:^|\s)active(?!\S)/g, '');
		}
	}

	export function apiDemoResponseVisualModifier(show: boolean) {
		var apiDemoResponse = <HTMLElement> document.getElementById("api-demo-response");

		if (show) {
			apiDemoResponse.className += " active";
		} else {
			apiDemoResponse.className = apiDemoResponse.className.replace(/(?:^|\s)active(?!\S)/g, '');
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
	demoApiKey: string = "2qz5daQDW0R1cGsyVnjF3cePFYhHEJHsL5hqdfXAxUE=";

	constructor() { }
	
	doRequest(endpoint: string) {
		HomeVisualStateManager.apiDemoResponseVisualModifier(false);
		HomeVisualStateManager.loaderVisualModifier(true);

		$.ajax({
			url: this.path + endpoint,
			type: "GET",
			dataType: "json",
			success: function (data, textStatus, jqxhr) {
				document.getElementById("json-preview").textContent = JSON.stringify(data, null, 4).trim();
				document.getElementById("api-demo-response-header").textContent = "api response - http(200)";
			},
			error: function (jqxhr, textStatus, error) {
				document.getElementById("api-demo-response-header").textContent = "api response - http(" + jqxhr.status + ")";
				if ((<string> jqxhr.responseText).indexOf("{") == 0) {
					document.getElementById("json-preview").textContent = JsonHelpers.formatJsonString(<string> jqxhr.responseText).trim();
				} else {
					document.getElementById("json-preview").textContent = JSON.stringify(this.genericError, null, 4).trim();
				}
			},
			complete: function () {
				HomeVisualStateManager.loaderVisualModifier(false);
				HomeVisualStateManager.apiDemoResponseVisualModifier(true);

				document.getElementById("api-demo-response").style.display = "block";
				hljs.highlightBlock($('pre code')[0]);
			},
			beforeSend: this.setXhrHeaders
		})
	}

	setXhrHeaders(xhr: XMLHttpRequest) {
		xhr.setRequestHeader("Authorization", "bearer " + this.demoApiKey);
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
