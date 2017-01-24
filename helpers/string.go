package helpers

import (
	"strconv"
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

// StrToInt converts a string to an int with the specified Base and Bit. If there is an
// error, it will return that error with the default specified value.
func StrToInt(str string, def int64, base, bit int) (int64, error) {
	i, err := strconv.ParseInt(str, base, bit)
	if err != nil {
		return def, err
	}

	return i, nil
}
