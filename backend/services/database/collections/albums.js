import Collection from '../collection';

export type AlbumLabel = {
	name: string,
	foundedIn: number,
	location: string,
	website: string,
};

export type Album = {
	_id: string,
	createdAt: Date,
	updatedAt: ?Date,

	title: string,
	slug: string,
	description: string,
	length: number,
	recordedIn: number,
	releasedAt: Date,
	coverImage: string,
	label: AlbumLabel,
};

export default class Albums extends Collection<Album> {
	async setupIndexes(): Promise<void> {
		await Promise.all([
			this.index(['id'], { unique: true }),
			this.index(['slug'], { unique: true }),
		]);
	}

	async createOneSimple(key: string): Promise<Album> {
		return await this.createOne({
			key,
			createdAt: new Date(),
		});
	}
}
