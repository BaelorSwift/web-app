package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	s "github.com/baelorswift/api/services"
	"gopkg.in/gin-gonic/gin.v1"
)

// AlbumsController ..
type AlbumsController struct{}

const albumSafeName = "albums"

// GetByID ..
func (ctrl AlbumsController) GetByID(c *gin.Context) {
	svc := s.NewDatabaseService(albumSafeName)
	defer svc.Close()

	var album m.Album
	if svc.Db.First(&album, "id = ?", c.Param("id")); album.Title != "" {
		c.JSON(http.StatusOK, &album)
	} else {
		c.JSON(http.StatusNotFound, m.NewBaelorError("album_not_found", nil))
	}
}

// Get ..
func (ctrl AlbumsController) Get(c *gin.Context) {
	svc := s.NewDatabaseService(albumSafeName)
	defer svc.Close()

	var albums []m.Album
	svc.Db.Find(&albums)
	c.JSON(http.StatusOK, &albums)
}

// Post ..
func (ctrl AlbumsController) Post(c *gin.Context) {
	svc := s.NewDatabaseService(albumSafeName)
	defer svc.Close()

	// Validate Payload
	var album m.Album
	if c.BindJSON(&album) != nil {
		c.JSON(http.StatusBadRequest,
			m.NewBaelorError("invalid_json", nil))
		return
	}

	// Validate JSON
	valid, err := h.Validate(&album, albumSafeName)
	if !valid {
		c.JSON(http.StatusUnprocessableEntity,
			m.NewBaelorError("validation_failed", err))
		return
	}

	// Check album is unique
	if svc.Exists("title_slug = ?", &album.TitleSlug) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("album_already_exists", nil))
		return
	}

	// Insert into database
	album.Init()
	if svc.Db.Create(&album); svc.Db.NewRecord(album) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_album", nil))
		return
	}
	c.JSON(http.StatusCreated, &album)
}

// NewAlbumsController ..
func NewAlbumsController(r *gin.RouterGroup) {
	ctrl := new(AlbumsController)

	r.GET("albums", ctrl.Get)
	r.GET("albums/:id", ctrl.GetByID)
	r.POST("albums", ctrl.Post)
}
