package helpers

import (
	"fmt"

	"net/http"

	m "github.com/baelorswift/api/models"
	goSchema "github.com/xeipuuv/gojsonschema"
	"gopkg.in/gin-gonic/gin.v1"
)

func validateBinding(c *gin.Context, obj interface{}) bool {
	return c.BindJSON(&obj) == nil
}

func validateJSONSchema(obj interface{}, schemaName string) *goSchema.Result {
	path := "https://raw.githubusercontent.com/BaelorSwift/api/dev/schema/%s.json"
	schema := goSchema.NewReferenceLoader(fmt.Sprintf(path, schemaName))
	doc := goSchema.NewGoLoader(obj)

	result, err := goSchema.Validate(schema, doc)
	if err != nil {
		panic(err.Error())
	}

	return result
}

// ValidateJSON on an object based on gin binding, and JSON Schema
func ValidateJSON(c *gin.Context, obj interface{}, schemaName string) (int, *m.BaelorError) {
	var baelorError *m.BaelorError
	metadata := map[string][]string{}

	// Validate Gin JSON Binding
	bindErr := validateBinding(c, obj)
	if !bindErr {
		metadata["json"] = c.Errors.Errors()
		err := m.NewBaelorError("json_binding_failed", metadata)
		return http.StatusBadRequest, &err
	}

	// Validate JSON Schema
	schemaResult := validateJSONSchema(&obj, schemaName)
	if schemaResult.Valid() {
		return http.StatusOK, baelorError
	}

	for _, desc := range schemaResult.Errors() {
		fieldSlice := metadata[desc.Field()]
		metadata[desc.Field()] = append(fieldSlice, desc.Description())
	}

	err := m.NewBaelorError("validation_failed", metadata)
	return http.StatusUnprocessableEntity, &err
}
