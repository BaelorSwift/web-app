export function getDate() {
	const now = new Date();

	return now.toISOString().substring(0, 10);
}
