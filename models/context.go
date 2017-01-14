package models

import "github.com/jinzhu/gorm"

// Context ..
type Context struct {
	Db *gorm.DB
}
