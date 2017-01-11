package helpers

import (
	"fmt"

	goSchema "github.com/xeipuuv/gojsonschema"
)

// Validate an object based on a json schema
func Validate(obj interface{}, schemaName string) (bool, map[string][]string) {
	path := "https://raw.githubusercontent.com/BaelorSwift/api/dev/schema/%s.json"
	schema := goSchema.NewReferenceLoader(fmt.Sprintf(path, schemaName))
	doc := goSchema.NewGoLoader(obj)

	result, err := goSchema.Validate(schema, doc)
	if err != nil {
		fmt.Println(err.Error())
	}

	if result.Valid() {
		return true, nil
	}

	metadata := make(map[string][]string)
	for _, desc := range result.Errors() {
		fieldSlice := metadata[desc.Field()]
		metadata[desc.Field()] = append(fieldSlice, desc.Description())
	}

	return false, metadata
}
