import contentSecurityPolicy from './content-security-policy';

export default function headers(req, res, next) {
	const csp = contentSecurityPolicy();
	const headers = {
		'strict-transport-security': 'max-age=63072000; includeSubDomains; preload',
		'x-content-type-options': 'nosniff',
		'x-frame-options': 'DENY',
		'x-xss-protection': '1; mode=block',
	};

	/* eslint-disable no-process-env */
	if (!process.argv.includes('disable-csp'))
		headers['content-security-policy'] = csp;

	res.set(headers);
	next();
}
