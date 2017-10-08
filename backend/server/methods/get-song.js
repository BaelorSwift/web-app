import validator from './_validate';

const validate = validator('get-song');

export default async function (ctx) {
	const { app, input } = ctx;

	validate(input);

	return await app.getSong(input.songSlug);
}
