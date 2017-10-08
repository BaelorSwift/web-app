import React, { Component } from 'react';
import {
	Alert,
	Button,
	Container,
	Input,
	InputGroup,
	InputGroupAddon,
} from 'reactstrap';
import LoadingRipple from '../LoadingRipple';
import SyntaxHighlighter from '../SyntaxHighlighter';
import { connect } from 'react-redux';
import propTypes from 'prop-types';
import exact from 'prop-types-exact';
import AceEditor from 'react-ace';
import { fetchDemo } from '../../actions/demo'

import 'brace';
import './index.css';
import 'brace/mode/json';
import 'brace/theme/monokai';

const initialPayload = `{
	"album_slug": "1989"
}
`;

class Home extends Component {
	constructor(props) {
		super(props);

		this.dismissFlash = this.dismissFlash.bind(this);
		this.createFlash = this.createFlash.bind(this);
		this.handleChange = this.handleChange.bind(this);
		this.makeRequest = this.makeRequest.bind(this);

		this.state = {
			endpoint: '/2017-10-08/get_album',
			payload: initialPayload,
			flash: void 0,
		};
	}

	componentWillMount() {
		window.document.title = 'Baelor.io';
	}
	
	dismissFlash() {
		if (!this.state.flash)
			return;
		
		this.setState({
			flash: {
				...this.state.flash,
				visible: false,
			}
		});
	}

	createFlash(colour, title, body) {
		this.setState({
			flash: {
				visible: true,
				body,
				colour,
				title,
			},
		});
	}

	handleChange(name, proxy) {
		switch (name) {
			case 'endpoint':
				this.setState({ endpoint: proxy.target.value });
				break;

			case 'payload':
				this.setState({ payload: proxy });
				break;

			default:
				throw new Error('Unknown change name');
		}
	}

	makeRequest() {
		let payload = void 0;
		let endpoint = this.state.endpoint;

		// Basic endpoint validation
		if (!endpoint) {
			this.createFlash(
				'warning',
				'Missing endpoint',
				'You must specify an endpoint to continue.',
			)

			return;
		}

		// Request payload validation
		try {
			payload = JSON.parse(this.state.payload);
		} catch (error) {
			this.createFlash(
				'warning',
				'Unable to parse JSON',
				`There was an issue parsing the JSON you entered: ${error}`
			);

			return;
		}

		// If we made it this far, hide all flash messages
		this.dismissFlash();

		// Dispatch fetch request
		this.props.dispatch(fetchDemo(endpoint, payload));
	}

	render() {
		const flash = this.state.flash;
		const demo = this.props.demo;

		return (
			<div className={'home-container'}>
				<section className={'hero'}>
					<video
						autoPlay
						loop
					>
						<source
							src={'/videos/baelor-home.mp4'}
						/>
						<img
							alt={'Taylor Swift'}
							src={'/images/baelor-cover.jpg'}
							title={'Your browser does not support the <video> tag'}
						/>
					</video>
					<div className={'mask'}>
						<h1>baelor.io</h1>
						<h2>The <strong>RPC</strong><sup>ish</sup> api for Taylor Swift</h2>
					</div>
				</section>
				<section className={'api-demo'}>
					<Container>
						<h3>{'API demo'}</h3>

						{flash &&
							<Alert
								color={flash.colour}
								isOpen={flash.visible}
								toggle={this.dismissFlash}
							>
								<strong>
									{flash.title}
								</strong>
								<br />
								{flash.body}
							</Alert>
						}

						<InputGroup>
							<InputGroupAddon>{'https://api.baelor.io/1'}</InputGroupAddon>
							<Input
								value={this.state.endpoint}
								onChange={this.handleChange.bind(this, 'endpoint')}
							/>
						</InputGroup>

						<SyntaxHighlighter
							title={'API request payload'}
							onChange={this.handleChange.bind(this, 'payload')}
							readOnly={false}
							value={this.state.payload}	
						/>

						<Button
							className={'shake-it-off'}
							color={'success'}
							onClick={this.makeRequest}
						>
							{'Shake it off!'}
						</Button>

						{demo.isLoading &&
							<LoadingRipple
								style={{
									transform: 'scale(0.5)',
								}}
							/>
						}

						{demo.payload &&
							<SyntaxHighlighter
								title={'Api response payload'}
								maxLines={100}
								value={JSON.stringify(demo.payload, null, '  ')}
							/>
						}
					</Container>
				</section>
			</div>
		);
	}
}

Home.propTypes = exact({
	demo: propTypes.object,
	dispatch: propTypes.func,
	history: propTypes.any,
	location: propTypes.any,
	match: propTypes.any,
	staticContext: propTypes.any,
});

function mapStateToProps(state) {
	return {
		demo: state.demo,
	};
}

export default connect(mapStateToProps)(Home);
