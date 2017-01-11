package controllers

import (
	"net/http"

	"log"

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
	svc.Find(&albums)

	c.JSON(http.StatusOK, albums)
}

// AlbumGet ..
func AlbumGet(c *gin.Context) {}

// AlbumsPost ..
func AlbumsPost(c *gin.Context) {
	svc := s.NewDatabaseService(AlbumSafeName)
	defer svc.Close()

	var album m.Album
	c.BindJSON(&album)

	// Validate JSON
	valid, err := h.Validate(&album, AlbumSafeName)
	if !valid {
		log.Fatal(err)
	}

	// Check album is unique
	if svc.Exists(&album) {
		log.Fatal("album already exists")
	}

	album.ID = uuid.NewV4().String()
	if !svc.Insert(&album) {
		log.Fatal("error saving album")
	}

	c.JSON(http.StatusOK, album)
}
