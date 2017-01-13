package main

import (
	"log"

	c "github.com/baelorswift/api/controllers"
	m "github.com/baelorswift/api/middleware"
	raven "github.com/getsentry/raven-go"
	"github.com/gin-contrib/sentry"
	"github.com/jinzhu/configor"

	"gopkg.in/gin-gonic/gin.v1"
)

// Config contains the loaded configuration
var Config = struct {
	Address string
	Dsn     string
}{}

func main() {
	configor.Load(&Config, "config/app.json")
	raven.SetDSN(Config.Dsn)

	r := gin.New()
	r.Use(gin.Logger(), gin.Recovery(), m.JSONOnly())
	r.Use(sentry.Recovery(raven.DefaultClient, false))
	v1 := r.Group("v1")
	{
		c.NewAlbumsController(v1)
		c.NewGenresController(v1)
		c.NewPeopleController(v1)
	}

	log.Fatal(r.Run(Config.Address))
}
