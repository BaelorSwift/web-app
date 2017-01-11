package services

import (
	m "github.com/baelorswift/api/models"
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/mysql"
)

// DatabaseService ..
type DatabaseService struct {
	table string
	db    *gorm.DB
}

// Find ..
func (svc DatabaseService) Find(out interface{}, where ...interface{}) {
	svc.db.Table(svc.table).Find(out, where)
}

// Close the database after you have finished with the service
func (svc DatabaseService) Close() {
	svc.db.Close()
}

// Exists checks if a certain model exists in the database
func (svc DatabaseService) Exists(obj interface{}) bool {
	count := 0
	svc.db.Table(svc.table).Model(&obj).Count(&count)
	return count >= 1
}

// Insert a new record into the database
func (svc DatabaseService) Insert(obj interface{}) bool {
	return svc.db.Table(svc.table).NewRecord(obj)
}

// NewDatabaseService ..
func NewDatabaseService(table string) (svc DatabaseService) {
	svc = DatabaseService{}

	db, err := gorm.Open("mysql", "baelor-admin:F88g2H69bj2669E6ZKDr@tcp(127.0.0.1:3306)/baelor_api")
	svc.db = db
	svc.table = table
	svc.db.LogMode(true)

	if err != nil {
		panic(err)
	}

	// Check Album Database Exists
	if !svc.db.HasTable(&m.Album{}) {
		svc.db.Set("gorm:table_options", "ENGINE=InnoDB").CreateTable(&m.Album{})
	}

	return
}
