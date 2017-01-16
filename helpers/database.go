package helpers

import (
	m "github.com/baelorswift/api/models"
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/mysql" // Required for gorm to deal with mysql
)

// NewDatabase ..
func NewDatabase(connectionStr string) *gorm.DB {
	db, err := gorm.Open("mysql", connectionStr)
	db.LogMode(true)

	if err != nil {
		panic(err)
	}

	// Run those migrations
	db.AutoMigrate(&m.Album{}, &m.Genre{}, &m.Label{}, &m.Person{})
	db.AutoMigrate(&m.Studio{}, &m.Song{}, &m.Lyric{})

	return db
}

// CheckIDsExist ..
func CheckIDsExist(ids []string, db *gorm.DB, wrongIdsCh chan map[string][]string, jsonFieldName string) {
	wrongIds := map[string][]string{}
	var items []m.Audit
	db.Where("id IN (?)", ids).Select("id").Find(&items)

	if len(items) == 0 {
		wrongIds[jsonFieldName] = ids
	} else if len(items) != len(ids) {
		foundIds := make([]string, len(items))
		for index, item := range items {
			foundIds[index] = item.ID
		}

		wrongIds[jsonFieldName] = Difference(ids, foundIds)
	}

	wrongIdsCh <- wrongIds
}
