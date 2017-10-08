import { composeWithDevTools } from 'redux-devtools-extension';
import createSagaMiddleware from 'redux-saga';
import rootReducer from './reducers';
import rootSaga from './sagas';
import { applyMiddleware, createStore } from 'redux';

function configureStore() {
	const sagaMiddleware = createSagaMiddleware();
	const store = createStore(rootReducer, composeWithDevTools(applyMiddleware(sagaMiddleware)));

	sagaMiddleware.run(rootSaga);
	window.store = store;

	return store;
}

export default configureStore;
