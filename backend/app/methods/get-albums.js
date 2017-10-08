import { Album } from '../../services/database/collections/albums';

export default async function getAlbum(): Promise<Album> {
	return await this.database.albums.all();
}
