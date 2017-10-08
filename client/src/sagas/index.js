import { watcherDemo } from './watchers/demo';
import { all } from 'redux-saga/effects';

export default function* rootSaga() {
	yield all([
		watcherDemo(),
	]);
}
