/* eslint-disable no-process-env, no-process-exit, func-style */

import 'babel-polyfill';
import App from '../app';
import Server from '../server';
import log from 'cuvva-log';
import * as Services from '../services';

const defaultPort = 3001;
const port = process.env.PORT || defaultPort;

log.setMinLogLevel('debug');
log.setHandler('fatal', () => process.exit(1));

const run = async () => {
	const config = {
		port: port,
	};

	const database = await Services.Database.connect({ uri: 'mongodb://localhost/baelor' });
	const app = new App(database);
	const server = new Server(app, { ...config });

	await server.setup();
	server.run();
};

(async () => {
	try {
		await run();
	} catch (error) {
		log.fatal('start_failed', [error]);
	}
})();
