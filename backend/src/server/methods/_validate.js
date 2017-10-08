import log from 'cuvva-log';
import validator from 'is-my-json-valid/require';

export default function createValidator(name) {
	const validate = validator(name, { greedy: true });

	return input => {
		if (validate(input))
			return;

		const reasons = validate.errors.map(e => log.debug('invalid_field', e));

		throw log.info('invalid_body', reasons);
	};
}
