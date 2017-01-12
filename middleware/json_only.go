package middleware

import "gopkg.in/gin-gonic/gin.v1"
import "strings"
import "net/http"
import h "github.com/baelorswift/api/helpers"
import m "github.com/baelorswift/api/models"

var relevantHTTPMethods = []string{"POST", "PUT", "PATCH"}

// JSONOnly ..
func JSONOnly() gin.HandlerFunc {
	return func(c *gin.Context) {
		if !h.InArray(relevantHTTPMethods, c.Request.Method) {
			c.Next()
			return
		}

		if strings.ToLower(c.ContentType()) != "application/json" {
			c.JSON(http.StatusUnsupportedMediaType, m.NewBaelorError("unsupported_media_type", nil))
			c.Abort()
		}
	}
}
