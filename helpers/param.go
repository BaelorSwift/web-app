package helpers

import "regexp"
import "fmt"

const (
	columnNameFormat = "%s_slug"
	typeIDKey        = "id"
	typeSlugKey      = "slug"
)

// DetectParamType ..
func DetectParamType(param, columnName string) (string, string) {
	r := regexp.MustCompile(`(?i)(?P<Type>slug|id)\((?P<Identifier>[a-z0-9\-]+)\)`)
	matched := r.FindStringSubmatch(param)
	if len(matched) < 3 {
		return "id", param
	}

	switch matched[1] {
	case typeIDKey:
		return typeIDKey, matched[2]
	case typeSlugKey:
		return fmt.Sprintf(columnNameFormat, columnName), matched[2]
	default:
		panic("impossible outcome")
	}
}
