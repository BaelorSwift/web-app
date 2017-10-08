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

	// create one document
	// returns new document
	async createOne(input: T): Promise<T> {
		const mappedInput = mapObjIn(input, true, this);
		const result = await this._collection.insertOne(mappedInput);

		return mapObjOut(result.ops[0], this);
	}

	// creates many documents
	// returns new documents
	async createMany(input: T[]): Promise<T[]> {
		const mappedInput = input.map(i => mapObjIn(i, true, this));
		const result = await this._collection.insertMany(mappedInput);

		return result.ops.map(o => mapObjOut(o, this));
	}

	// retrieve one document by id
	// returns found document
	async retrieveOne(id: string): Promise<T> {
		const mappedId = parseIdForFind(id, this);

		return await this.findOne({ _id: mappedId });
	}

	// retrieves many documents by id
	// returns found documents
	async retrieveMany(ids: string[]): Promise<T[]> {
		const mappedIds = ids.map(i => parseIdForFind(i, this));

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

	// update one document by id
	// returns updated document
	// input not mapped because Mongo operators are used
	async updateOne(id: string, update: {}): Promise<T> {
		const filter = { _id: parseIdForFind(id, this) };
		const options = { returnOriginal: false };
		const result = await this._collection.findOneAndUpdate(filter, update, options);

		if (!result.ok || !result.value)
			throw log.warn('not_found', { result });

		return mapObjOut(result.value, this);
	}

	// update many documents by filter
	// returns number of documents updated
	// input not mapped because Mongo operators are used
	async updateMany(filter: {}, update: {}): Promise<number> {
		const result = await this._collection.updateMany(filter, update);

		return result.matchedCount;
	}

	// create or update one document by filter
	// returns new or updated document
	// input not mapped because Mongo operators are used
	async upsertOne(filter: {}, update: {}): Promise<T> {
		const options = { upsert: true, returnOriginal: false };
		const result = await this._collection.findOneAndUpdate(filter, update, options);

		if (!result.ok || !result.value)
			throw log.warn('not_found', { result });

		return mapObjOut(result.value, this);
	}

	// todo: is upsertMany possible?

	// delete one document by id
	async deleteOne(id: string): Promise<void> {
		const mappedId = parseIdForFind(id, this);
		const result = await this._collection.deleteOne({ _id: mappedId });

		if (!result.deletedCount)
			throw log.info('not_found', { result });
	}

	// deletes many documents by filter
	// returns number of documents deleted
	// input not mapped because Mongo operators are used
	async deleteMany(filter: {}): Promise<number> {
		const result = await this._collection.deleteMany(filter);

		return result.deletedCount;
	}
}

function parseIdForFind<T>(input: string, obj: Collection<T>): {} {
	const idp = obj.constructor.idProvider;

	try {
		return idp.parse(input);
	} catch (error) {
		if (error.code === 'invalid_id')
			throw log.info('not_found');
		throw error;
	}
}

// maps input from the application to the MongoDB fields
function mapObjIn<T>(input: T, idNeeded: boolean, obj: Collection<T>): {} {
	if (input._id)
		throw log.error('id_rules_violated', { input });

	// shallow copy
	const inputCopy = { ...input };

	const idp = obj.constructor.idProvider;

	if (inputCopy.id) {
		inputCopy._id = idp.parse(inputCopy.id);
		inputCopy.id = void 0;
	}

	if (!inputCopy._id && idNeeded)
		inputCopy._id = idp.generate();

	return obj.mapInput(inputCopy);
}

// maps MongoDB output to the application-friendly format
function mapObjOut<T>(output: {}, obj: Collection<T>): T {
	if (output.id)
		throw log.error('id_rules_violated', { output });

	// shallow copy
	const outputCopy = { ...output };

	if (output._id) {
		const idp = obj.constructor.idProvider;

		outputCopy.id = idp.stringify(outputCopy._id);
		outputCopy._id = void 0;
	}

	return obj.mapOutput(outputCopy);
}
