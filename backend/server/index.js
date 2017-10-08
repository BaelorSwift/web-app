import Express from 'express';
import camelcase from 'camelcase';
import camelcaseRecursive from 'camelcase-keys-recursive';
import log from 'cuvva-log';
import path from 'path';
import snakecaseKeys from 'snakeize';
import * as Methods from './methods';
import * as Middleware from './middleware';

export default class Server {
	constructor(app, options = {}) {
		this.app = app;
		this.express = Express();
		this.options = options;
	}

	async setup() {
		await this._setupMiddleware();
	}

	run() {
		this.express.listen(this.options.port);

		log.info('server_listening', { port: this.options.port });
	}

	async _setupMiddleware() {
		const e = this.express;

		e.use(Middleware.headers);
		e.use(Middleware.types);
		e.use(Middleware.body);
		e.get('/system/health', wrap(this._healthCheck, this));
		e.use('/api/1/:version/:method', wrap(this._handler, this));
		e.get('/*', (req, res) => res.sendFile(path.join(__dirname, '../', '/client/build/index.html')));
		e.use(Middleware.notFound);
		e.use(Middleware.error);
	}

	_healthCheck(req, res) {
		res.sendStatus(204);
	}

	async _handler(req, res) {
		const method = Methods[camelcase(req.params.method)];
		const date = getVersionDate(req.params.version);

		if (!method)
			throw log.info('function_not_found');

		if (date === null)
			throw log.info('preview_not_available');

		if (req.method.toLowerCase() !== 'post')
			throw log.info('method_not_allowed');

		const output = await method({
			app: this.app,
			input: camelcaseRecursive(req.body),
		});

		if (output === void 0 || output === null) {
			res.status(204);
			res.end();

			return;
		}

		res.status(200);
		res.json(snakecaseKeys(output));
	}
}

function wrap(handler, thisArg) {
	return (req, res, next) => {
		(async () => {
			try {
				await handler.call(thisArg, req, res);

				if (!res.headersSent)
					next();
			} catch (error) {
				next(error);
			}
		})();
	};
}

function getVersionDate(version) {
	switch (version) {
		case 'latest': return new Date();
		case 'preview': return null;

		default: {
			if (!/^\d{4}-\d{2}-\d{2}$/.test(version))
				throw log.info('invalid_version');

			try {
				return new Date(version);
			} catch (error) {
				throw log.info('invalid_version');
			}
		}
	}
}
