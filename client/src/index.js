import 'bootstrap/dist/css/bootstrap.css';
import './index.css';

import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import Routes from './routes';
import configureStore from './store';
import registerServiceWorker from './registerServiceWorker';

const app = (
	<Provider store={configureStore()}>
		<Routes />
	</Provider>
);

ReactDOM.render(app, document.getElementById('root'));
registerServiceWorker();
