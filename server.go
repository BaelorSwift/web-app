package main

import (
	"log"

	c "github.com/baelorswift/api/controllers"
	h "github.com/baelorswift/api/helpers"
	middleware "github.com/baelorswift/api/middleware"
	m "github.com/baelorswift/api/models"
	raven "github.com/getsentry/raven-go"
	"github.com/gin-contrib/sentry"
	"github.com/jinzhu/configor"

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

	context := &m.Context{Db: h.NewDatabase(Config.ConnectionString)}

	r := gin.New()
	r.Use(gin.Logger(), gin.Recovery())
	r.Use(middleware.JSONOnly())
	r.Use(sentry.Recovery(raven.DefaultClient, false))
	r.Static("/static", "./static/")
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
