import { demo } from '../../api/demo';
import { FETCH_DATA_FAILURE, FETCH_DATA_SUCCESS } from '../../constants/action-types';
import { call, put } from 'redux-saga/effects';

export function* workerDemo(action) {
	try {
		const { endpoint, payload } = action.payload;
		const response = yield call(demo, endpoint, payload);

		yield put({ type: FETCH_DATA_SUCCESS, payload: response });
	} catch (error) {
		yield put({ type: FETCH_DATA_FAILURE, payload: error });
	}
}
