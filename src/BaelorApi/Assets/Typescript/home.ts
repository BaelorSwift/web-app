/// <reference path="Definitions/jquery.d.ts" />

class ApiStuff {
	path: string = "/api/v0/";

	constructor() {

	}

	doRequest(endpoint: string) {
		$.getJSON(this.path + endpoint, function (data) {

		})
		.fail(function (jqxhr, textStatus, error) {

		});
	}
}

window.onload = () => {
	var api = new ApiStuff();
};