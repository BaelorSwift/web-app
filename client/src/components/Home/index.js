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
import { fetchDemo } from '../../actions/demo'
import { getDate } from '../../helpers/date';
import exact from 'prop-types-exact';
import propTypes from 'prop-types';
import './index.css';

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
			endpoint: 'get_album',
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
						poster={'/images/baelor-cover.jpg'}
					>
						<source
							src={'/videos/baelor-home.webm'}
							type={'video/webm'}
						/>
						<source
							src={'/videos/baelor-home.mp4'}
							type={'video/mp4'}
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

						<InputGroup className={'d-none d-md-flex desktop-input'}>
							<InputGroupAddon>{`https://api.baelor.io/${getDate()}/`}</InputGroupAddon>
							<Input
								value={this.state.endpoint}
								onChange={this.handleChange.bind(this, 'endpoint')}
							/>
						</InputGroup>

						<div className={'d-md-none mobile-input'}>
							<div className={'prefix'}>{`https://api.baelor.io/${getDate()}/`}</div>
							<Input
								value={this.state.endpoint}
								onChange={this.handleChange.bind(this, 'endpoint')}
							/>
						</div>

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
								title={'API response payload'}
								maxLines={40}
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
