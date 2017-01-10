package main

import (
	"net/http"

	"github.com/bahlo/goat"
	"github.com/getsentry/raven-go"
	"github.com/jinzhu/configor"
)

// Config contains the loaded configuration
var Config = struct {
	DSN string
}{}

func helloHandler(w http.ResponseWriter, r *http.Request, p goat.Params) {
	nameQuery := r.URL.Query().Get("name")

	goat.WriteJSON(w, map[string]string{
		"hello": nameQuery,
	})
}

func main() {
	configor.Load(&Config, "config.json")
	raven.SetDSN(Config.DSN)

	r := goat.New()
	r.Get("/", "home", helloHandler)
	r.Run(":3000")
}
