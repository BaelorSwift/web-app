import initialState from './initial-state';
import {
	FETCH_DATA,
	FETCH_DATA_FAILURE,
	FETCH_DATA_SUCCESS,
} from '../constants/action-types';

export default function (state = initialState.demo, action) {
	switch (action.type) {
		case FETCH_DATA:
			return {
				...initialState.demo,
				isLoading: true,
			};

		case FETCH_DATA_FAILURE:
		return {
			error: action.payload,
			isLoading: false,
		};
		
		case FETCH_DATA_SUCCESS:
			return {
				payload: action.payload,
				isLoading: false,
			};

		default: return state;
	}
}
