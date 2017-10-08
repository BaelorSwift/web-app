import { FETCH_DATA } from '../../constants/action-types';
import { takeEvery } from 'redux-saga/effects';
import { workerDemo } from '../workers/demo';

export function* watcherDemo() {
	window.console.info(`redux-saga: [watcherDemo] started :: listening for ${FETCH_DATA};`);

	yield takeEvery(FETCH_DATA, workerDemo);
}
