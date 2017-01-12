package main

import (
	"log"

	c "github.com/baelorswift/api/controllers"
	raven "github.com/getsentry/raven-go"
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

	r := gin.Default()
	//r.Use(sentry.Recovery(raven.DefaultClient, false))
	v1 := r.Group("v1")
	{
		c.NewAlbumsController(v1)
		c.NewGenresController(v1)
	}

	log.Fatal(r.Run(Config.Address))
}
