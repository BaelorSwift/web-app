package main

import (
	"log"
	"time"

	c "github.com/baelorswift/api/controllers"
	h "github.com/baelorswift/api/helpers"
	middleware "github.com/baelorswift/api/middleware"
	m "github.com/baelorswift/api/models"
	raven "github.com/getsentry/raven-go"
	"github.com/gin-contrib/sentry"
	"github.com/jinzhu/configor"
	cache "github.com/patrickmn/go-cache"
	"gopkg.in/gin-contrib/cors.v1"
	"gopkg.in/gin-gonic/gin.v1"
)

// Config contains the loaded configuration
var Config = struct {
	Address          string `json:"address"`
	DSN              string `json:"dsn"`
	ConnectionString string `json:"connection_string"`
}{}

func main() {
	configor.Load(&Config, "config/app.json")
	raven.SetDSN(Config.DSN)

	r := gin.New()
	r.RedirectTrailingSlash = true
	context := &m.Context{
		Db:    h.NewDatabase(Config.ConnectionString, gin.Mode() != gin.ReleaseMode),
		Cache: cache.New(60*time.Minute, 30*time.Second),
	}

	// Setup CORS
	r.Use(cors.New(cors.Config{
		AllowOrigins:     []string{"http://localhost:3001", "https://baelor.io", "https://taylorswift.io/"},
		AllowMethods:     []string{"GET", "POST", "PUT", "PATCH", "DELETE"},
		AllowHeaders:     []string{"Origin"},
		ExposeHeaders:    []string{"Content-Length"},
		AllowCredentials: true,
		AllowOriginFunc: func(origin string) bool {
			return origin == "https://github.com"
		},
		MaxAge: 12 * time.Hour,
	}))

	r.Use(gin.Logger(), gin.Recovery())
	r.Use(middleware.RequestAnalytics(context))
	r.Use(middleware.JSONOnly(), middleware.IPRateLimit(context))
	r.Use(sentry.Recovery(raven.DefaultClient, false))
	r.Static("/static", "./static/")
	r.StaticFile("/favicon.ico", "./static/favicon.ico")
	v1 := r.Group("v1")
	{
		c.NewAlbumsController(v1, context)
		c.NewGenresController(v1, context)
		c.NewPeopleController(v1, context)
		c.NewLabelsController(v1, context)
		c.NewStudiosController(v1, context)
		c.NewSongsController(v1, context)
		c.NewLyricsController(v1, context)
		c.NewUsersController(v1, context)
	}

	log.Fatal(r.Run(Config.Address))
}
