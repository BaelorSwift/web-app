import log from 'cuvva-log';
import { ObjectID as oid } from 'mongodb';

export default {
	generate,
	parse,
	stringify,
};

function generate(): {} {
	return oid();
}

function parse(str): ?{} {
	if (str === void 0 || str === null)
		return null;

	if (str instanceof oid)
		return str;

	try {
		return oid(str);
	} catch (error) {
		throw log.info('invalid_id', [error]);
	}
}

function stringify(obj): ?string {
	if (obj === void 0 || obj === null)
		return null;

	if (obj instanceof String)
		return obj;

	return obj.toString();
}
