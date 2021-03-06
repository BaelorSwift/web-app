/* eslint-disable no-process-env, no-process-exit, func-style */

import 'babel-polyfill';
import App from '../app';
import Server from '../server';
import log from 'cuvva-log';
import logSentry from 'cuvva-log-sentry';
import raven from 'raven';
import * as Services from '../services';

const defaultPort = 3000;
const port = process.env.PORT || defaultPort;
const config = JSON.parse(process.env.CONFIG);

const sentry = new raven.Client(config.sentryDSN);

log.setHandler(logSentry(sentry));
log.setHandler('fatal', logSentry(sentry, () => process.exit(1)));

const run = async () => {
	const database = await Services.Database.connect({ uri: config.mongo.uri});
	const app = new App(database);
	const server = new Server(app, { port, ...config });

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
