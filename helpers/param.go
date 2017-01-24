package helpers

import (
	"fmt"
	"regexp"
	"strings"
)

// DetectParamType ..
func DetectParamType(param string, identTypes map[string]string) (string, string) {
	identTypeKeys := make([]string, len(identTypes))
	for key := range identTypes {
		identTypeKeys = append(identTypeKeys, key)
	}

	r := regexp.MustCompile(fmt.Sprintf(`(?i)(?P<Type>%s)\((?P<Identifier>[a-z0-9\-]+)\)`, strings.Join(identTypeKeys, "|")))
	matched := r.FindStringSubmatch(param)
	if len(matched) < 3 {
		return "id", param
	}

	for key, value := range identTypes {
		if matched[1] == key {
			return value, matched[2]
		}
	}

	panic("impossibru outcome")
}
