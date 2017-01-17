package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// LyricsController ..
type LyricsController struct {
	context *m.Context
}

const lyricSafeName = "lyrics"

// Get ..
func (ctrl LyricsController) Get(c *gin.Context) {
	var lyrics []m.Lyric
	ctrl.context.Db.Preload("Song").Find(&lyrics)
	response := make([]*m.LyricResponse, len(lyrics))
	for i, lyric := range lyrics {
		response[i] = lyric.Map()
	}
	c.JSON(http.StatusOK, &response)
}

// Post ..
func (ctrl LyricsController) Post(c *gin.Context) {
	// Validate Payload
	var lyric m.Lyric
	status, err := h.ValidateJSON(c, &lyric, lyricSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check ids exist
	wrongIdsCh := make(chan map[string][]string)
	go h.CheckIDsExist([]string{lyric.SongID}, ctrl.context.Db.Table("songs"), wrongIdsCh, "song_id")
	wrongIds := h.UnionMaps(<-wrongIdsCh)
	if len(wrongIds) > 0 {
		c.JSON(http.StatusUnprocessableEntity, m.NewBaelorError("invalid_ids", wrongIds))
		return
	}

	// Insert into database
	lyric.Init()
	ctrl.context.Db.Where("id = ?", &lyric.SongID).First(&lyric.Song)
	if ctrl.context.Db.Create(&lyric); ctrl.context.Db.NewRecord(lyric) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_lyric", nil))
		return
	}

	c.JSON(http.StatusCreated, lyric.Map())
}

// NewLyricsController ..
func NewLyricsController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(LyricsController)
	ctrl.context = c

	r.GET("lyrics", ctrl.Get)
	r.POST("lyrics", middleware.BearerAuth(c), ctrl.Post)
}
