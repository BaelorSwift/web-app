package middleware

import (
	"net/http"
	"strings"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

var relevantHTTPMethods = []string{"POST", "PUT", "PATCH"}

// JSONOnly ..
func JSONOnly() gin.HandlerFunc {
	return func(c *gin.Context) {
		if !helpers.InSlice(relevantHTTPMethods, c.Request.Method) {
			c.Next()
			return
		}

		if strings.ToLower(c.ContentType()) != "application/json" {
			c.JSON(http.StatusUnsupportedMediaType, models.NewBaelorError("unsupported_media_type", nil))
			c.Abort()
		}
	}
}
