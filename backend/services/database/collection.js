import { Collection as MCollection } from 'mongodb';
import { MongoNativeId } from './id-providers';
import log from 'cuvva-log';

export default class Collection<T> {
	_collection: MCollection;

	// usually overridden by subclasses
	static idProvider = MongoNativeId;
	async setupIndexes(): Promise<void> { /**/ }
	mapInput(input: T): {} { return input; }
	mapOutput(output: {}): T { return output; }

	constructor(collection: MCollection) {
		this._collection = collection;
	}

	async index(keys: string[]|{}[]|{}, options: ?{}): Promise<void> {
		if (!Array.isArray(keys)) {
			await this._collection.createIndex(keys, options);

			return;
		}

		const specs = keys.map(k => {
			if (typeof k === 'object')
				return k;

			const spec = {};

			spec[k] = 1;

			return spec;
		});

		// eslint-disable-next-line array-callback-return
		await Promise.all(specs.map(async s => {
			await this._collection.createIndex(s, options);
		}));
	}

	// gets all the documents in the
	// collection
	async all(): Promise<T[]> {
		return await this.findMany({ });
	}

	// retrieve one document by id
	// returns found document
	async retrieveOne(id: string): Promise<T> {
		const mappedId = this.parseIdForFind(id);

		return await this.findOne({ _id: mappedId });
	}

	// retrieves many documents by id
	// returns found documents
	async retrieveMany(ids: string[]): Promise<T[]> {
		const mappedIds = ids.map(i => this.parseIdForFind(i));

		return await this.findMany({ _id: { $in: mappedIds } });
	}

	// find one document by filter
	// returns found document
	// input not mapped because Mongo operators are used
	async findOne(filter: {}): Promise<T> {
		const object = await this._collection.find(filter).next();

		if (!object)
			throw log.info('not_found');

		return mapObjOut(object, this);
	}

	// find many documents by filter
	// returns array of found documents
	// input not mapped because Mongo operators are used
	async findMany(filter: {}): Promise<T[]> {
		const results = await this._collection.find(filter).toArray();

		return results.map(o => mapObjOut(o, this));
	}

	// count documents by filter
	// returns number of matching documents
	// input not mapped because Mongo operators are used
	async count(filter: {}): Promise<number> {
		return await this._collection.count(filter);
	}

	parseIdForFind(input: string): {} {
		const idp = this.constructor.idProvider;

		try {
			return idp.parse(input);
		} catch (error) {
			if (error.code === 'invalid_id')
				throw log.info('not_found');
			throw error;
		}
	}
}

// maps MongoDB output to the application-friendly format
function mapObjOut<T>(output: {}, obj: Collection<T>): T {
	if (output.id)
		throw log.error('id_rules_violated', { output });

	// shallow copy
	const outputCopy = { ...output };

	// Fix up all ids
	Object
		.keys(outputCopy)
		.filter(k => k === '_id' || k.endsWith('Id'))
		.forEach(k => {
			const provider = obj.constructor.idProvider;
			const id = outputCopy[k];
			const fixedId = provider.stringify(id);

			if (k === '_id') {
				outputCopy.id = fixedId;
				delete outputCopy._id;
			} else {
				outputCopy[k] = fixedId;
			}
		});

	return obj.mapOutput(outputCopy);
}
