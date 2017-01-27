package helpers

import (
	"encoding/json"
	"fmt"

	"bytes"
	"net/http"

	"github.com/baelorswift/api/models"
	goSchema "github.com/xeipuuv/gojsonschema"
	"gopkg.in/gin-gonic/gin.v1"
)

const schemaPath = "file:///schema/%s.json"

func validateJSONSchema(bodyData []byte, schemaName string) *goSchema.Result {
	schema := goSchema.NewReferenceLoader(fmt.Sprintf(schemaPath, schemaName))
	doc := goSchema.NewBytesLoader(bodyData)

	result, err := goSchema.Validate(schema, doc)
	if err != nil {
		panic(err.Error())
	}

	return result
}

// ValidateJSON on an object based on gin binding, and JSON Schema
func ValidateJSON(c *gin.Context, obj interface{}, schemaName string) (int, *models.BaelorError) {
	var baelorError *models.BaelorError
	metadata := map[string][]string{}

	// Read string Body
	buf := new(bytes.Buffer)
	buf.ReadFrom(c.Request.Body)
	bodyData := buf.Bytes()

	// Validate JSON Schema
	schemaResult := validateJSONSchema(bodyData, schemaName)
	if schemaResult.Valid() {
		json.Unmarshal(buf.Bytes(), obj)
		return http.StatusOK, baelorError
	}

	for _, desc := range schemaResult.Errors() {
		fieldSlice := metadata[desc.Field()]
		metadata[desc.Field()] = append(fieldSlice, desc.Description())
	}

	err := models.NewBaelorError("validation_failed", metadata)
	return http.StatusUnprocessableEntity, &err
}
