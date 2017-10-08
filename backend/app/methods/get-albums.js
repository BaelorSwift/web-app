import { Album } from '../../services/database/collections/albums';
import App from '../';
import log from 'cuvva-log';

export default async function getAlbum(): Promise<Album> {
	return await findAll(this);
}

async function findAll(app: App) {
	try {
		return await app.database.albums.findMany({ });
	} catch (error) {
		if (error.code === 'not_found')
			throw log.info('document_not_found');

		throw error;
	}
}
