package models

import (
	raven "github.com/getsentry/raven-go"
	"github.com/jinzhu/gorm"
	cache "github.com/patrickmn/go-cache"
)

// Context ..
type Context struct {
	Db    *gorm.DB
	Cache *cache.Cache
	Raven *raven.Client
}
