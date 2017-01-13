package helpers

import (
	"strings"
	"unicode"
)

// GenerateSlug creates a slug based on a string
// Taken from: https://github.com/mrvdot/golang-utils/blob/master/utils.go#L23
func GenerateSlug(str string) string {
	return strings.Map(func(r rune) rune {
		switch {
		case r == ' ', r == '-':
			return '-'
		case r == '_', unicode.IsLetter(r), unicode.IsDigit(r):
			return r
		default:
			return -1
		}

	}, strings.ToLower(strings.TrimSpace(str)))
}
