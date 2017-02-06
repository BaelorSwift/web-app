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
	db.LogMode(!release)

	if err != nil {
		panic(err)
	}

	// Run those dank migrations
	db.AutoMigrate(&models.Album{}, &models.Genre{}, &models.Label{}, &models.Person{})
	db.AutoMigrate(&models.Studio{}, &models.Song{}, &models.Lyric{}, &models.User{})
	db.AutoMigrate(&models.Analytic{})

	return db
}

// FindWithPagination retrieves start and count (offset and limit) from the url query,
// loads in preloads and pulls the results from the database
func FindWithPagination(db *gorm.DB, out interface{}, c *gin.Context, safeName string, preloads ...string) (start, count int64) {
	// Parse start and count from query string
	start, _ = StrToInt(c.DefaultQuery("start", "0"), 0, 10, 32)
	count, _ = StrToInt(c.DefaultQuery("count", "25"), 25, 10, 32)

	// Force start and count to be within bounds
	if count > 100 {
		count = 100
	}
	if count <= 0 {
		count = 1
	}
	if start < 0 {
		start = 0
	}

	// Construct query
	query := db.Offset(start).Limit(count).Order("created_at desc")
	for _, preload := range preloads {
		query = query.Preload(preload)
	}
	query.Find(out)
	return
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
