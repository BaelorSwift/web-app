package controllers

import (
	"fmt"
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// AlbumsController ..
type AlbumsController struct {
	context    *models.Context
	identTypes map[string]string
}

const albumSafeName = "albums"

// Get ..
func (ctrl AlbumsController) Get(c *gin.Context) {
	var albums []models.Album
	preloads := []string{"Songs", "Genres", "Producers", "Studios", "Label"}
	start, count := helpers.FindWithPagination(ctrl.context.Db, &albums, c, albumSafeName, preloads...)
	response := make([]*models.AlbumResponse, len(albums))
	for i, album := range albums {
		response[i] = album.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, albumSafeName, start, count))
}

// GetByIdent ..
func (ctrl AlbumsController) GetByIdent(c *gin.Context) {
	var album models.Album
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)

	// I don't like this, at all. it's awful
	query := ctrl.context.Db.Preload("Genres").Preload("Producers").Preload("Studios")
	query = query.Preload("Label").Preload("Songs")

	if query.First(&album, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("album_not_found", nil))
	} else {
		c.JSON(http.StatusOK, album.Map())
	}
}

// Post ..
func (ctrl AlbumsController) Post(c *gin.Context) {
	// Validate Payload
	var album models.Album
	status, err := helpers.ValidateJSON(c, &album, albumSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check album is unique
	album.TitleSlug = helpers.GenerateSlug(album.Title)
	if !ctrl.context.Db.First(&models.Album{}, "title_slug = ?", &album.TitleSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("album_already_exists", nil))
		return
	}

	// Check ids exist
	wrongIdsCh := make(chan map[string][]string)
	go helpers.CheckIDsExist(album.ProducerIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "producer_ids")
	go helpers.CheckIDsExist(album.GenreIDs, ctrl.context.Db.Table("genres"), wrongIdsCh, "genre_ids")
	go helpers.CheckIDsExist(album.StudioIDs, ctrl.context.Db.Table("studios"), wrongIdsCh, "studio_ids")
	go helpers.CheckIDsExist([]string{album.LabelID}, ctrl.context.Db.Table("labels"), wrongIdsCh, "label_id")
	wrongIds := helpers.UnionMaps(<-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh)
	if len(wrongIds) > 0 {
		c.JSON(http.StatusUnprocessableEntity, models.NewBaelorError("invalid_ids", wrongIds))
		return
	}

	// Insert into database
	album.Init()
	ctrl.context.Db.Where("id IN (?)", album.ProducerIDs).Find(&album.Producers)
	ctrl.context.Db.Where("id IN (?)", album.GenreIDs).Find(&album.Genres)
	ctrl.context.Db.Where("id IN (?)", album.StudioIDs).Find(&album.Studios)
	ctrl.context.Db.Where("id = ?", album.LabelID).First(&album.Label)
	if ctrl.context.Db.Create(&album); ctrl.context.Db.NewRecord(album) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_album", nil))
		return
	}

	c.JSON(http.StatusCreated, album.Map())
}

// Delete ..
func (ctrl AlbumsController) Delete(c *gin.Context) {
	var album models.Album
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)
	if ctrl.context.Db.First(&album, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("album_not_found", nil))
		return
	}

	errs := ctrl.context.Db.Delete(&album).GetErrors()
	if len(errs) == 0 {
		c.Status(http.StatusNoContent)
	} else {
		ctrl.context.Raven.CaptureError(errs[0], nil)
		c.JSON(http.StatusInternalServerError, models.NewBaelorError("unknown_error_deleting_album", nil))
	}
}

// NewAlbumsController ..
func NewAlbumsController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(AlbumsController)
	ctrl.context = c
	ctrl.identTypes = map[string]string{
		"id":   "id",
		"slug": "title_slug",
	}

	r.GET("albums", ctrl.Get)
	r.GET("albums/:ident", ctrl.GetByIdent)
	r.POST("albums", middleware.BearerAuth(c), ctrl.Post)
	r.DELETE("albums/:ident", middleware.BearerAuth(c), ctrl.Delete)
}
