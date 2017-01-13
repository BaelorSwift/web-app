package main

import (
	"log"

	c "github.com/baelorswift/api/controllers"
	m "github.com/baelorswift/api/middleware"
	s "github.com/baelorswift/api/services"
	raven "github.com/getsentry/raven-go"
	"github.com/gin-contrib/sentry"
	"github.com/jinzhu/configor"

	"gopkg.in/gin-gonic/gin.v1"
)

// Config contains the loaded configuration
var Config = struct {
	Address          string `json:"address"`
	Dsn              string `json:"dsn"`
	ConnectionString string `json:"connectionString"`
}{}

func main() {
	configor.Load(&Config, "config/app.json")
	raven.SetDSN(Config.Dsn)
	s.ConnectionString = Config.ConnectionString

	r := gin.New()
	r.Use(gin.Logger(), gin.Recovery(), m.JSONOnly())
	r.Use(sentry.Recovery(raven.DefaultClient, false))
	v1 := r.Group("v1")
	{
		c.NewAlbumsController(v1)
		c.NewGenresController(v1)
		c.NewPeopleController(v1)
		c.NewLabelsController(v1)
	}

	log.Fatal(r.Run(Config.Address))
}
