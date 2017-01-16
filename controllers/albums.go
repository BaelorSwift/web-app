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

// Get ..
func (ctrl AlbumsController) Get(c *gin.Context) {
	var albums []m.Album
	ctrl.context.Db.Preload("Songs").Preload("Genres").Preload("Producers").Preload("Studios").Preload("Label").Find(&albums)
	response := make([]*m.AlbumResponse, len(albums))
	for i, album := range albums {
		response[i] = album.Map()
	}
	c.JSON(http.StatusOK, &response)
}

// GetByID ..
func (ctrl AlbumsController) GetByID(c *gin.Context) {
	var album m.Album

	// I don't like this, at all. it's awful
	query := ctrl.context.Db.Preload("Genres").Preload("Producers").Preload("Studios")
	query = query.Preload("Label").First(&album, "id = ?", c.Param("id")).Preload("Songs")

	if query.RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("album_not_found", nil))
	} else {
		c.JSON(http.StatusOK, album.Map())
	}
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
	album.TitleSlug = h.GenerateSlug(album.Title)
	if !ctrl.context.Db.First(&m.Album{}, "title_slug = ?", &album.TitleSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, m.NewBaelorError("album_already_exists", nil))
		return
	}

	// Check ids exist
	wrongIdsCh := make(chan map[string][]string)
	go h.CheckIDsExist(album.ProducerIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "producer_ids")
	go h.CheckIDsExist(album.GenreIDs, ctrl.context.Db.Table("genres"), wrongIdsCh, "genre_ids")
	go h.CheckIDsExist(album.StudioIDs, ctrl.context.Db.Table("studios"), wrongIdsCh, "studio_ids")
	go h.CheckIDsExist([]string{album.LabelID}, ctrl.context.Db.Table("labels"), wrongIdsCh, "label_id")
	wrongIds := h.UnionMaps(<-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh)
	if len(wrongIds) > 0 {
		c.JSON(http.StatusUnprocessableEntity, m.NewBaelorError("invalid_ids", wrongIds))
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
			m.NewBaelorError("unknown_error_creating_album", nil))
		return
	}

	c.JSON(http.StatusCreated, album.Map())
}

// NewAlbumsController ..
func NewAlbumsController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(AlbumsController)
	ctrl.context = c

	r.GET("albums", ctrl.Get)
	r.GET("albums/:id", ctrl.GetByID)
	r.POST("albums", ctrl.Post)
}
