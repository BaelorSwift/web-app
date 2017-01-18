package middleware

import (
	"bytes"
	"strings"

	"github.com/baelorswift/api/models"
	uuid "github.com/satori/go.uuid"
	"gopkg.in/gin-gonic/gin.v1"
)

const (
	analyticIDContextKey = "analytic_id"
)

type responseWriter struct {
	gin.ResponseWriter
	body *bytes.Buffer
}

func (w responseWriter) Write(b []byte) (int, error) {
	w.body.Write(b)
	return w.ResponseWriter.Write(b)
}

// RequestAnalytics ..
func RequestAnalytics(context *models.Context) gin.HandlerFunc {
	return func(c *gin.Context) {
		requestID := uuid.NewV4().String()
		requestIP := c.Request.Header.Get("CF-Connecting-IP")
		if requestIP == "" {
			requestIP = c.ClientIP()
		}

		// Set analytics id, and create struct with request data
		c.Set(analyticIDContextKey, requestID)
		analytics := &models.Analytic{
			RequestID:          requestID,
			RequestIP:          requestIP,
			RequestMethod:      strings.ToUpper(c.Request.Method),
			RequestPath:        c.Request.URL.Path,
			RequestPayloadSize: c.Request.ContentLength,
		}

		// Create custom writer wrapper, and replace the gin writer with it
		writer := &responseWriter{body: bytes.NewBufferString(""), ResponseWriter: c.Writer}
		c.Writer = writer

		// Go to next middleware
		c.Next()

		// Populate analytics model from response data
		analytics.ResponseStatus = c.Writer.Status()
		analytics.ResponsePayloadSize = c.Writer.Size()

		// Init and write analytics data to database
		analytics.Init()
		context.Db.Create(analytics)
	}
}
