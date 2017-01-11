package helpers

import goSchema "github.com/xeipuuv/gojsonschema"
import "fmt"
import "os"

// Validate an object based on a json schema
func Validate(obj interface{}, schemaName string) (bool, []goSchema.ResultError) {
	fmt.Println(os.Args[0])

	schema := goSchema.NewReferenceLoader("schema/albums.json") //fmt.Sprintf("schema/%s.json", schemaName))
	doc := goSchema.NewGoLoader(&obj)

	result, err := goSchema.Validate(schema, doc)
	if err != nil {
		fmt.Println(err.Error())
	}

	if result.Valid() {
		return true, nil
	}

	return false, result.Errors()
}
