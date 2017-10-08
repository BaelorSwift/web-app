import { noop } from 'lodash';
import React, { Component } from 'react';
import exact from 'prop-types-exact';
import propTypes from 'prop-types';

import 'brace';
import './index.css';
import 'brace/mode/json';
import 'brace/theme/monokai';

import AceEditor from 'react-ace';

class SyntaxHighlighter extends Component {
	render() {
		return (
			<div className={'syntax-highlighter'}>
				<div className={'header'}>
					{this.props.title}
				</div>
		
				<AceEditor
					mode={'json'}
					theme={'monokai'}
					onChange={this.props.onChange}
					fontSize={14}
					value={this.props.value}
					showGutter={true}
					readOnly={this.props.readOnly}
					highlightActiveLine={false}
					maxLines={this.props.maxLines}
					minLines={this.props.minLines}
					wrapEnabled={true}
					scrollMargin={[16, 16, 0, 0]}
					tabSize={2}
					showPrintMargin={false}
					setOptions={{
						enableBasicAutocompletion: true,
						enableLiveAutocompletion: true,
						enableSnippets: false,
						showLineNumbers: false,
						scrollPastEnd: true,
					}}
				/>
			</div>
		);
	}
}

SyntaxHighlighter.propTypes = exact({
	title: propTypes.string.isRequired,
	onChange: propTypes.func,
	readOnly: propTypes.bool,
	maxLines: propTypes.number,
	minLines: propTypes.number,
	value: propTypes.string,
});

SyntaxHighlighter.defaultProps = {
	onChange: noop,
	maxLines: 10,
	minLines: 6,
	readOnly: true,
};

export default SyntaxHighlighter;
