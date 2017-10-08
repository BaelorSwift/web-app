import React from 'react';
import App from './components/App';
import Home from './components/Home';
import { Route, BrowserRouter as Router, Switch } from 'react-router-dom';

function routes() {
	return (
		<Router>
			<App>
				<Switch>
					<Route
						component={Home}
						exact
						path={'/'}
					/>
				</Switch>
			</App>
		</Router>
	);
}

export default routes;
