package middleware

import (
	"strings"

	"net/http"

	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

const (
	// AuthUserIDKey is the key in the gin context that stores the id of the authenticated user
	AuthUserIDKey = "authenticated_user"
	tokenType     = "bearer "
)

// BearerAuth ..
func BearerAuth(context *m.Context) gin.HandlerFunc {
	return func(c *gin.Context) {
		err := m.NewBaelorError("invalid_authentication", nil)
		token := c.Request.Header.Get("Authorization")

		// Check auth starts with 'bearer'
		tokenLower := strings.ToLower(token)
		if !strings.HasPrefix(tokenLower, tokenType) {
			c.JSON(http.StatusUnauthorized, err)
			c.Abort()
			return
		}

		// Get api key and compare with database
		user := &m.User{}
		query := context.Db.First(user, "api_key = ?", token[len(tokenType):])
		if query.RecordNotFound() {
			c.JSON(http.StatusUnauthorized, err)
			c.Abort()
			return
		}

		// Set user id to gin context
		c.Set(AuthUserIDKey, user.ID)
	}
}
