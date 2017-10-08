import { Album } from './songs';
import Collection from '../collection';

export type Song = {
	_id: string,
	createdAt: Date,
	updatedAt: ?Date,

	index: number,
	title: string,
	slug: string,
	length: number,
	isSingle: boolean,
	albumId: string,
	album: Album,
};

export default class Songs extends Collection<Song> {
	async setupIndexes(): Promise<void> {
		await Promise.all([
			this.index(['slug'], { unique: true }),
			this.index(['albumId']),
		]);
	}

	async retrieveManyByAlbumId(albumId: string): Promise<Song[]> {
		const mappedId = this.parseIdForFind(albumId);

		try {
			return await this.findMany({ albumId: mappedId });
		} catch (error) {
			if (error.code === 'not_found')
				return [];

			throw error;
		}
	}
}
