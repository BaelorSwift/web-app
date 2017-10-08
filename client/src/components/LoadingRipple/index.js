import React, { Component } from 'react';
import './index.css';

class LoadingRipple extends Component {
	render() {
		return (
			<div
				className={'loading-ripple'}
				style={this.props.style}
			>
				<div
					style={{
						height: '100%',
						width: '100%',
					}}
					className={'ripple'}
				>
					<div></div>
					<div></div>
				</div>
			</div>
		);
	}
}

LoadingRipple.defaultProps = {
	style: {},
};

export default LoadingRipple;
