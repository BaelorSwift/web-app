import log from 'cuvva-log';
import * as Collections from './collections';
import { Db, MongoClient } from 'mongodb';

type Config = {
	uri: string;
};

export default class Database {
	db: Db;
	collections: {}[];

	static async connect(config: Config): Promise<Database> {
		// for this particular service, majority is worth having
		// if copying, evaluate your write concern needs properly
		const db = await MongoClient.connect(config.uri);

		log.info('database_connected');

		return new Database(db);
	}

	constructor(db) {
		this.db = db;
		this.collections = [];

		for (const [key, Class] of Object.entries(Collections)) {
			const mongoCollection = db.collection(key);
			const instance = new Class(mongoCollection);

			this[key] = instance;
			this.collections.push(instance);
		}

		// not awaited
		setupIndexes(this.collections);
	}
}

async function setupIndexes(collections): Promise<void> {
	try {
		await Promise.all(collections.map(c => c.setupIndexes()));
		log.info('database_indexes_setup');
	} catch (error) {
		log.error('index_creation_failed', [error]);
	}
}
