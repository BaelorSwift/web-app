import { Song } from '../../services/database/collections/songs';

export default async function getSongs(): Promise<Song> {
	return await this.database.songs.all();
}
