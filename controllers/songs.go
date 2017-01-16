package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// SongsController ..
type SongsController struct {
	context *m.Context
}

const songSafeName = "songs"

// Get ..
func (ctrl SongsController) Get(c *gin.Context) {
	var songs []m.Song
	ctrl.context.Db.Preload("Album").Preload("Producers").Preload("Writers").Preload("Genres").First(&songs)
	response := make([]*m.SongResponse, len(songs))
	for i, song := range songs {
		response[i] = song.Map()
	}
	c.JSON(http.StatusOK, &response)
}

// GetBySlug ..
func (ctrl SongsController) GetBySlug(c *gin.Context) {
	var song m.Song

	query := ctrl.context.Db.Preload("Album").Preload("Producers").Preload("Writers")
	query = query.Preload("Genres").First(&song, "title_slug = ?", c.Param("slug"))

	if query.RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("song_not_found", nil))
	} else {
		c.JSON(http.StatusOK, song.Map())
	}
}

// Post ..
func (ctrl SongsController) Post(c *gin.Context) {
	// Validate Payload
	var song m.Song
	status, err := h.ValidateJSON(c, &song, songSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check song is unique
	song.TitleSlug = h.GenerateSlug(song.Title)
	if !ctrl.context.Db.First(&m.Song{}, "title_slug = ?", &song.TitleSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, m.NewBaelorError("song_already_exists", nil))
		return
	}

	// Check ids exist
	wrongIdsCh := make(chan map[string][]string)
	go h.CheckIDsExist(song.ProducerIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "producer_ids")
	go h.CheckIDsExist(song.WriterIDs, ctrl.context.Db.Table("people"), wrongIdsCh, "writer_ids")
	go h.CheckIDsExist(song.GenreIDs, ctrl.context.Db.Table("genres"), wrongIdsCh, "genre_ids")
	go h.CheckIDsExist([]string{song.AlbumID}, ctrl.context.Db.Table("albums"), wrongIdsCh, "album_id")
	wrongIds := h.UnionMaps(<-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh, <-wrongIdsCh)
	if len(wrongIds) > 0 {
		c.JSON(http.StatusUnprocessableEntity, m.NewBaelorError("invalid_ids", wrongIds))
		return
	}

	// Insert into database
	song.Init()
	ctrl.context.Db.Where("id IN (?)", song.ProducerIDs).Find(&song.Producers)
	ctrl.context.Db.Where("id IN (?)", song.GenreIDs).Find(&song.Genres)
	ctrl.context.Db.Where("id IN (?)", song.WriterIDs).Find(&song.Writers)
	ctrl.context.Db.Where("id = ?", song.AlbumID).First(&song.Album)
	if ctrl.context.Db.Create(&song); ctrl.context.Db.NewRecord(song) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_song", nil))
		return
	}

	c.JSON(http.StatusCreated, song.Map())
}

// NewSongsController ..
func NewSongsController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(SongsController)
	ctrl.context = c

	r.GET("songs", ctrl.Get)
	r.GET("songs/:slug", ctrl.GetBySlug)
	r.POST("songs", ctrl.Post)
}
