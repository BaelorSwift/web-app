package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// AlbumsController ..
type AlbumsController struct {
	context *m.Context
}

const albumSafeName = "albums"

// GetByID ..
func (ctrl AlbumsController) GetByID(c *gin.Context) {
	var album m.Album
	if ctrl.context.Db.First(&album, "id = ?", c.Param("id")).RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("album_not_found", nil))
	} else {
		c.JSON(http.StatusOK, &album)
	}
}

// Get ..
func (ctrl AlbumsController) Get(c *gin.Context) {
	var albums []m.Album
	ctrl.context.Db.Find(&albums)
	c.JSON(http.StatusOK, &albums)
}

// Post ..
func (ctrl AlbumsController) Post(c *gin.Context) {
	// Validate Payload
	var album m.Album
	status, err := h.ValidateJSON(c, &album, albumSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check album is unique
	if !ctrl.context.Db.Where("title_slug = ?", &album.TitleSlug).RecordNotFound() {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("album_already_exists", nil))
		return
	}

	// Insert into database
	album.Init()
	if ctrl.context.Db.Create(&album); ctrl.context.Db.NewRecord(album) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_album", nil))
		return
	}
	c.JSON(http.StatusCreated, &album)
}

// NewAlbumsController ..
func NewAlbumsController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(AlbumsController)
	ctrl.context = c

	r.GET("albums", ctrl.Get)
	r.GET("albums/:id", ctrl.GetByID)
	r.POST("albums", ctrl.Post)
}
