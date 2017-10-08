export default function contentSecurityPolicy() {
	const opts = {
		default: ['\'none\''],
		connect: [
			'\'self\'',
			'https://*',
		],
		font: [
			'data:',
			'https://*',
		],
		img: [
			'\'self\'',
			'data:',
			'https://*',
		],
		media: ['https://*'],
		frame: [],
		script: [
			'\'self\'',
			'\'unsafe-eval\'',
		],
		style: [
			'\'unsafe-inline\'',
			'https://*',
		],
	};
	const directives = Object.keys(opts).map(k => `${k}-src ${opts[k].join(' ')}`);

	return directives.join(';');
}
