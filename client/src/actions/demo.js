import { FETCH_DATA } from '../constants/action-types';

export function fetchDemo(endpoint: string, payload: {}) {
	return {
		type: FETCH_DATA,
		payload: {
			endpoint,
			payload,
		},
	};
}
