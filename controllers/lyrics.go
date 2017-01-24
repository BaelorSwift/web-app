package controllers

import (
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// LyricsController ..
type LyricsController struct {
	context *models.Context
}

const lyricSafeName = "lyrics"

// Get ..
func (ctrl LyricsController) Get(c *gin.Context) {
	var lyrics []models.Lyric
	start, count := helpers.FindWithPagination(ctrl.context.Db, &lyrics, c, lyricSafeName)
	response := make([]*models.LyricResponse, len(lyrics))
	for i, lyric := range lyrics {
		response[i] = lyric.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, lyricSafeName, start, count))
}

// Post ..
func (ctrl LyricsController) Post(c *gin.Context) {
	// Validate Payload
	var lyric models.Lyric
	status, err := helpers.ValidateJSON(c, &lyric, lyricSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check ids exist
	wrongIdsCh := make(chan map[string][]string)
	go helpers.CheckIDsExist([]string{lyric.SongID}, ctrl.context.Db.Table("songs"), wrongIdsCh, "song_id")
	wrongIds := helpers.UnionMaps(<-wrongIdsCh)
	if len(wrongIds) > 0 {
		c.JSON(http.StatusUnprocessableEntity, models.NewBaelorError("invalid_ids", wrongIds))
		return
	}

	// Check there isn't already a lyric with this index and song_id
	query := ctrl.context.Db.First(&models.Lyric{}, "`lyrics`.song_id = ? AND `lyrics`.index = ?", lyric.SongID, lyric.Index)
	if !query.RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("lyric_line_already_exists", nil))
		return
	}

	// Insert into database
	lyric.Init()
	ctrl.context.Db.Where("id = ?", &lyric.SongID).First(&lyric.Song)
	if ctrl.context.Db.Create(&lyric); ctrl.context.Db.NewRecord(lyric) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_lyric", nil))
		return
	}

	c.JSON(http.StatusCreated, lyric.Map())
}

// NewLyricsController ..
func NewLyricsController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(LyricsController)
	ctrl.context = c

	r.GET("lyrics", ctrl.Get)
	r.POST("lyrics", middleware.BearerAuth(c), ctrl.Post)
}
