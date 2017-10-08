import { getDate } from '../helpers/date';

const API_BASE = `/api/${getDate()}/`;

export function demo(endpoint: string, payload: {}) {
	const opts = {
		body: JSON.stringify(payload),
		credentials: 'same-origin',
		headers: {
			'content-type': 'application/json',
			'accept': 'application/json'
		},
		method: 'POST',
	};

	if (payload) opts.headers['content-type'] = 'application/json';

	return fetch(API_BASE + endpoint, opts)
		.then(resp => resp.json());
}
