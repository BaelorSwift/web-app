package models

import (
	"github.com/jinzhu/gorm"
	cache "github.com/patrickmn/go-cache"
)

// Context ..
type Context struct {
	Db    *gorm.DB
	Cache *cache.Cache
}
