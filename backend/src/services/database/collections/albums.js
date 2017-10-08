import Collection from '../collection';
import { Song } from './songs';
import log from 'cuvva-log';

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
	songs: Song[],
};

export default class Albums extends Collection<Album> {
	async setupIndexes(): Promise<void> {
		await Promise.all([
			this.index(['slug'], { unique: true }),
		]);
	}
}
