import { Song } from '../../services/database/collections/songs';

export default async function getSong(slug: string): Promise<Song> {
	const song = await this.database.songs.findOne({ slug });

	song.album = await this.database.albums.retrieveOne(song.albumId);
	song.albumId = void 0;

	return song;
}
