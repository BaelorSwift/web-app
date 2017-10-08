import validator from './_validate';

const validate = validator('get-albums');

export default async function (ctx) {
	const { app, input } = ctx;

	validate(input);

	return await app.getAlbums();
}
