/// <reference path="Definitions/jquery.d.ts" />
/// <reference path="Definitions/highlightjs.d.ts" />
declare class ApiStuff {
    path: string;
    constructor();
    doRequest(endpoint: string): void;
}
