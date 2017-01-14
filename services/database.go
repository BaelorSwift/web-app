package services

import (
	m "github.com/baelorswift/api/models"
	"github.com/jinzhu/gorm"

	// so we can actually look at our mysql db
	_ "github.com/jinzhu/gorm/dialects/mysql"
)

// NewDatabase ..
func NewDatabase(connectionStr string) *gorm.DB {
	db, err := gorm.Open("mysql", connectionStr)
	db.LogMode(true)

	if err != nil {
		panic(err)
	}

	// Check Album Table Exists
	if !db.HasTable(&m.Album{}) {
		db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Album{})
	}

	// Check Genre Table Exists
	if !db.HasTable(&m.Genre{}) {
		db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Genre{})
	}

	// Check Person Table Exists
	if !db.HasTable(&m.Person{}) {
		db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Person{})
	}

	// Check Label Table Exists
	if !db.HasTable(&m.Label{}) {
		db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Label{})
	}

	return db
}
