package middleware

import (
	"fmt"
	"time"

	"net/http"

	m "github.com/baelorswift/api/models"
	cache "github.com/patrickmn/go-cache"
	"gopkg.in/gin-gonic/gin.v1"
)

const (
	cacheIPKey = "ip:%s:hr:%d"
	rateLimit  = 1000

	rateLimitLimitHeader     = "X-Rate-Limit-Limit"
	rateLimitRemainingHeader = "X-Rate-Limit-Remaining"
	rateLimitResetHeader     = "X-Rate-Limit-Reset"
)

// IPRateLimit ..
func IPRateLimit(context *m.Context) gin.HandlerFunc {
	return func(c *gin.Context) {
		// Check if we need to get the client up from cloudflare
		clientIP := c.Request.Header.Get("CF-Connecting-IP")
		if clientIP == "" {
			clientIP = c.ClientIP()
		}
		now := time.Now().UTC()
		clientIPKey := fmt.Sprintf(cacheIPKey, clientIP, now.Hour())

		// Get current count, and create item if it doesn't exist
		count, exists := context.Cache.Get(clientIPKey)
		if !exists {
			context.Cache.Set(clientIPKey, 1, cache.DefaultExpiration)
			count = 1
		} else {
			context.Cache.IncrementInt(clientIPKey, 1)
			count = count.(int) + 1
		}

		// Remaining Count
		remaining := rateLimit - count.(int)
		if remaining < 0 {
			remaining = 0
			context.Cache.Set(clientIPKey, rateLimit, cache.DefaultExpiration)
		}

		// Set rate limit headers
		nextHour := time.Now().UTC().Add(time.Hour)
		nextHourStart := time.Date(nextHour.Year(), nextHour.Month(), nextHour.Day(), nextHour.Hour(), 0, 0, 0, time.UTC)
		c.Header(rateLimitLimitHeader, fmt.Sprintf("%d", rateLimit))
		c.Header(rateLimitRemainingHeader, fmt.Sprintf("%d", remaining))
		c.Header(rateLimitResetHeader, nextHourStart.String())

		// Check if we hit that rate limit
		if remaining == 0 {
			c.JSON(http.StatusTooManyRequests, m.NewBaelorError("rate_limit_exceeded", nil))
			c.Abort()
			return
		}
	}
}
