export default function (req, res, next) {
	if (req.accepts('json') || req.accepts('html'))
		next();
	else
		// eslint-disable-next-line no-magic-numbers
		res.sendStatus(406);
}
