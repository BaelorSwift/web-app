/// <reference path="Definitions/jquery.d.ts" />
declare class ApiStuff {
    path: string;
    constructor();
    doRequest(endpoint: string): void;
}
