import { Router } from 'express';
import { hasBody } from 'type-is';
import log from 'cuvva-log';
import { json, urlencoded } from 'body-parser';

const router = Router();

export default router;

router.use(json());
router.use(urlencoded({ extended: true }));
router.use(checkBody);

function checkBody(req, res, next) {
	// has body, but wasn't parsed
	if (hasBody(req) && !req.body)
		next(log.info('unacceptable_content_type'));
	else
		next();
}
