package helpers

// UnionMaps ..
func UnionMaps(maps ...map[string][]string) map[string][]string {
	out := map[string][]string{}
	for _, m := range maps {
		for j, k := range m {
			out[j] = k
		}
	}
	return out
}
