import React, { Component } from 'react';
import {
	Container,
	Collapse,
	Navbar,
	NavbarToggler,
	Nav,
	NavItem,
	NavLink,
} from 'reactstrap';
import { connect } from 'react-redux';
import { withRouter } from 'react-router';
import propTypes from 'prop-types';
import exact from 'prop-types-exact';
import './index.css';

class App extends Component {
	constructor(props) {
		super(props);
	
		this.toggleNavbar = this.toggleNavbar.bind(this);

		this.state = {
			collapsed: true
		};
	}
	
	toggleNavbar() {
		this.setState({
			collapsed: !this.state.collapsed
		});
	}
	
	render() {
		return (
			<div className={'app'}>
				<Navbar
					color={'light'}
					expand={'md'}
					light
				>
					<NavbarToggler
						onClick={this.toggleNavbar}
						className={'mr-2'}
					/>
					<Collapse
						isOpen={!this.state.collapsed}
						navbar
					>
						<Nav
							className={'mr-auto'}
							navbar
						>
							<NavItem>
								<NavLink href={'/'}>Home</NavLink>
							</NavItem>
							<NavItem>
								<NavLink href={'/docs'}>Docs</NavLink>
							</NavItem>
							<NavItem>
								<NavLink href={'/about'}>About</NavLink>
							</NavItem>
						</Nav>
					</Collapse>
				</Navbar>
				
				{this.props.children}

				<footer>
					<hr />
					<Container>
						<ul>
							<li>
								{'Made by '}
								<a href={'https://twitter.com/0xdeafcafe'} target={'_blank'}>
									{'Alex Forbes-Reed - @0xdeafcafe'}
								</a>
							</li>
							<li>
								{'Written in NodeJS, using '}
								<a href={'https://expressjs.com/'} target={'_blank'}>
									{'Express'}
								</a>
								{', and '}
								<a href={'https://reactjs.org/'} target={'_blank'}>
									{'React'}
								</a>
								{'.'}
							</li>
							<li>
								{'Code licensed under the '}
								<a href={'https://www.dbad-license.org/'} target={'_blank'}>
									{'Don\'t Be A Dick Public License'}
								</a>
							</li>
							<li>
								{'Code open sourced at '}
								<a href={'https://github.com/BaelorSwift'} target={'_blank'}>
									{'github.com/BaelorSwift'}
								</a>
							</li>
						</ul>

						<p className={'legal'}>
							© <span id={'copyright-year'}>2017</span>
						</p>
					</Container>
				</footer>
			</div>
		);
	}
}

App.propTypes = exact({
	children: propTypes.any,
	dispatch: propTypes.func,
	history: propTypes.any,
	location: propTypes.any,
	match: propTypes.any,
	staticContext: propTypes.any,
});

export default withRouter(connect()(App));
