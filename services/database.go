package services

import (
	m "github.com/baelorswift/api/models"
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/mysql" // so we can actually look at our mysql Db
)

// DatabaseService ..
type DatabaseService struct {
	table string
	Db    *gorm.DB
}

// ConnectionString is the connection string for the database
var ConnectionString string

// Close the database after you have finished with the service
func (svc DatabaseService) Close() {
	svc.Db.Close()
}

// Exists checks if a query results in a match in the database
func (svc DatabaseService) Exists(query interface{}, replacements ...interface{}) bool {
	count := 0
	svc.Db.Table(svc.table).Where(query, replacements).Count(&count)
	return count >= 1
}

// NewDatabaseService ..
func NewDatabaseService(table string) (svc DatabaseService) {
	svc = DatabaseService{}

	Db, err := gorm.Open("mysql", ConnectionString)
	svc.Db = Db
	svc.table = table
	svc.Db.LogMode(true)

	if err != nil {
		panic(err)
	}

	// Check Album Table Exists
	if !svc.Db.HasTable(&m.Album{}) {
		svc.Db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Album{})
	}

	// Check Genre Table Exists
	if !svc.Db.HasTable(&m.Genre{}) {
		svc.Db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Genre{})
	}

	// Check Person Table Exists
	if !svc.Db.HasTable(&m.Person{}) {
		svc.Db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Person{})
	}

	// Check Label Table Exists
	if !svc.Db.HasTable(&m.Label{}) {
		svc.Db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Label{})
	}

	return
}
