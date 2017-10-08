export default async function (ctx) {
	const { app } = ctx;

	return await app.getAlbums();
}
