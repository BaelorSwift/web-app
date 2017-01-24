package controllers

import (
	"net/http"

	"fmt"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// SongsController ..
type SongsController struct {
	context *models.Context
}

const songSafeName = "songs"

// Get all songs
func (ctrl SongsController) Get(c *gin.Context) {
	var songs []models.Song
	preloads := []string{"Album", "Producers", "Writers", "Genres", "Featuring"}
	start, count := helpers.FindWithPagination(ctrl.context.Db, &songs, c, songSafeName, preloads...)
	response := make([]*models.SongResponse, len(songs))
	for i, song := range songs {
		response[i] = song.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, songSafeName, start, count))
}

// GetByIdent ..
func (ctrl SongsController) GetByIdent(c *gin.Context) {
	var song models.Song
	identType, ident := helpers.DetectParamType(c.Param("ident"), "title")

	query := ctrl.context.Db.Preload("Album").Preload("Producers").Preload("Writers")
	query = query.Preload("Genres").Preload("Lyrics").Preload("Featuring")

	if query.First(&song, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("song_not_found", nil))
	} else {
		c.JSON(http.StatusOK, song.Map())
	}
}

// Post ..
func (ctrl SongsController) Post(c *gin.Context) {
	// Validate Payload
	var song models.Song
	status, err := helpers.ValidateJSON(c, &song, songSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check song is unique
	song.TitleSlug = helpers.GenerateSlug(song.Title)
	if !ctrl.context.Db.First(&models.Song{}, "title_slug = ?", &song.TitleSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("song_already_exists", nil))
		return
	}

	// Check ids exist
	wrongIdsCh := make(chan map[string][]string)
	go helpers.CheckIDsExist(song.ProducerIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "producer_ids")
	go helpers.CheckIDsExist(song.WriterIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "writer_ids")
	go helpers.CheckIDsExist(song.GenreIDs, ctrl.context.Db.Table("genres"), wrongIdsCh, "genre_ids")
	go helpers.CheckIDsExist(song.FeaturingIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "featuring_ids")
	go helpers.CheckIDsExist([]string{song.AlbumID}, ctrl.context.Db.Table("albums"), wrongIdsCh, "album_id")
	wrongIds := helpers.UnionMaps(<-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh)
	if len(wrongIds) > 0 {
		c.JSON(http.StatusUnprocessableEntity, models.NewBaelorError("invalid_ids", wrongIds))
		return
	}

	// Insert into database
	song.Init()
	ctrl.context.Db.Where("id IN (?)", song.ProducerIDs).Find(&song.Producers)
	ctrl.context.Db.Where("id IN (?)", song.GenreIDs).Find(&song.Genres)
	ctrl.context.Db.Where("id IN (?)", song.WriterIDs).Find(&song.Writers)
	ctrl.context.Db.Where("id IN (?)", song.FeaturingIDs).Find(&song.Featuring)
	ctrl.context.Db.Where("id = ?", song.AlbumID).First(&song.Album)
	if ctrl.context.Db.Create(&song); ctrl.context.Db.NewRecord(song) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_song", nil))
		return
	}

	c.JSON(http.StatusCreated, song.Map())
}

// NewSongsController ..
func NewSongsController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(SongsController)
	ctrl.context = c

	r.GET("songs", ctrl.Get)
	r.GET("songs/:ident", ctrl.GetByIdent)
	r.POST("songs", middleware.BearerAuth(c), ctrl.Post)
}
