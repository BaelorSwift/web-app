import validator from './_validate';

const validate = validator('get-album');


export default async function (ctx) {
	const { app, input } = ctx;

	validate(input);

	console.log(input);

	return await app.getAlbum(input.albumSlug);
}
