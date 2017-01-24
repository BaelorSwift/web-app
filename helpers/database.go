package helpers

import (
	"github.com/baelorswift/api/models"
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/mysql" // Required for gorm to deal with mysql
	"gopkg.in/gin-gonic/gin.v1"
)

// NewDatabase ..
func NewDatabase(connectionStr string, release bool) *gorm.DB {
	db, err := gorm.Open("mysql", connectionStr)
	db.LogMode(true)

	if err != nil {
		panic(err)
	}

	// Run those dank migrations
	db.AutoMigrate(&models.Album{}, &models.Genre{}, &models.Label{}, &models.Person{})
	db.AutoMigrate(&models.Studio{}, &models.Song{}, &models.Lyric{}, &models.User{})
	db.AutoMigrate(&models.Analytic{})

	return db
}

// CheckIDsExist ..
func CheckIDsExist(ids []string, db *gorm.DB, wrongIdsCh chan map[string][]string, jsonFieldName string) {
	if len(ids) == 0 {
		wrongIdsCh <- map[string][]string{}
		return
	}

	wrongIds := map[string][]string{}
	var items []models.Audit
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
