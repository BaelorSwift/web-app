import { Album } from '../../services/database/collections/albums';

export default async function getAlbum(slug: string): Promise<Album> {
	const album = await this.database.albums.findOne({ slug });
	const songs = await this.database.songs.retrieveManyByAlbumId(album.id);

	album.songs = songs.map(s => Object.assign({}, s, { albumId: void 0 }));

	return album;
}
