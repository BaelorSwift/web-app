import * as Methods from './methods';
import * as Services from '../services';

export default class App {
	database: Services.Database;

	constructor(database) {
		this.database = database;
	}
}

Object.assign(App.prototype, Methods);
