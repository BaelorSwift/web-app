import { Album } from '../../services/database/collections/albums';
import App from '../';
import log from 'cuvva-log';

export default async function getAlbum(slug: string): Promise<Album> {
	return await findBySlug(this, slug);
}

async function findBySlug(app: App, slug: string) {
	try {
		return await app.database.albums.findOne({ slug });
	} catch (error) {
		if (error.code === 'not_found')
			throw log.info('document_not_found');

		throw error;
	}
}
