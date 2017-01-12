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

	Db, err := gorm.Open("mysql", "baelor-admin:F88g2H69bj2669E6ZKDr@tcp(127.0.0.1:3306)/baelor_api")
	svc.Db = Db
	svc.table = table
	svc.Db.LogMode(true)

	if err != nil {
		panic(err)
	}

	// Check Album Database Exists
	if !svc.Db.HasTable(&m.Album{}) {
		svc.Db.Set("gorm:table_options", "ENGINE=InnoDb").CreateTable(&m.Album{})
	}

	return
}
