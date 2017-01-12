package controllers

import (
	"net/http"
	"time"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	s "github.com/baelorswift/api/services"
	uuid "github.com/satori/go.uuid"
	"gopkg.in/gin-gonic/gin.v1"
)

// AlbumSafeName contains the escaped name for the controller, used for selecting
// the correct SQL table and JSON schema
const AlbumSafeName = "albums"

// AlbumsGet ..
func AlbumsGet(c *gin.Context) {
	svc := s.NewDatabaseService(AlbumSafeName)
	defer svc.Close()

	var albums []m.Album
	svc.Db.Find(&albums)

	c.JSON(http.StatusOK, albums)
}

// AlbumGet ..
func AlbumGet(c *gin.Context) {}

// AlbumsPost ..
func AlbumsPost(c *gin.Context) {
	svc := s.NewDatabaseService(AlbumSafeName)
	defer svc.Close()

	var album m.Album

	// Validate Payload
	if c.BindJSON(&album) != nil {
		c.JSON(http.StatusUnprocessableEntity,
			m.NewBaelorError("invalid_json", map[string][]string{}))
		return
	}

	// Validate JSON
	valid, err := h.Validate(&album, AlbumSafeName)
	if !valid {
		c.JSON(http.StatusUnprocessableEntity,
			m.NewBaelorError("validation_failed", err))
		return
	}

	// Check album is unique
	if svc.Exists("title_slug = ?", album.TitleSlug) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("album_already_exists", map[string][]string{}))
		return
	}

	// Set metadata fields
	now := time.Now().UTC()
	album.ID = uuid.NewV4().String()
	album.CreatedAt = now
	album.UpdatedAt = now

	// Insert into database
	svc.Db.Create(&album)
	if svc.Db.NewRecord(album) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_album", map[string][]string{}))
		return
	}

	c.JSON(http.StatusCreated, album)
}
