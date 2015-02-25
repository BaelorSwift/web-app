/// <reference path="Definitions/jquery.d.ts" />
/// <reference path="Definitions\highlightjs.d.ts" />

class ApiStuff {
	path: string = "/api/v0/";

	constructor() {

	}

	doRequest(endpoint: string) {
		$.getJSON(this.path + endpoint, function (data) {
			document.getElementById("json-preview").innerText = JSON.stringify(data, null, 4).trim();
		})
		.fail(function (jqxhr, textStatus, error) {
			console.error(textStatus);
		})
			.always(function () {
			document.getElementById("api-demo-response").style.display = "block";
			hljs.highlightBlock($('pre code')[0]);
		});
	}
}

window.onload = () => {
	var api = new ApiStuff();

	document.getElementById("get-api-preview").onclick = () => {
		api.doRequest((<HTMLInputElement> document.getElementById("api-preview-path")).value);
	};
};
