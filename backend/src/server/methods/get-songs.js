import validator from './_validate';

const validate = validator('get-songs');

export default async function (ctx) {
	const { app, input } = ctx;

	validate(input);

	return await app.getSongs();
}
