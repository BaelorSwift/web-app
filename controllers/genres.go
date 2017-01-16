package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// GenresController ..
type GenresController struct {
	context *m.Context
}

const genreSafeName = "genres"

// Get ..
func (ctrl GenresController) Get(c *gin.Context) {
	var genres []m.Genre
	ctrl.context.Db.Find(&genres)
	response := make([]*m.GenreResponse, len(genres))
	for i, genre := range genres {
		response[i] = genre.Map()
	}
	c.JSON(http.StatusOK, &response)
}

// GetByID ..
func (ctrl GenresController) GetByID(c *gin.Context) {
	var genre m.Genre
	if ctrl.context.Db.First(&genre, "id = ?", c.Param("id")).RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("genre_not_found", nil))
	} else {
		c.JSON(http.StatusOK, genre.Map())
	}
}

// Post ...
func (ctrl GenresController) Post(c *gin.Context) {
	// Validate Payload
	var genre m.Genre
	status, err := h.ValidateJSON(c, &genre, genreSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check genre is unique
	genre.NameSlug = h.GenerateSlug(genre.Name)
	if !ctrl.context.Db.First(&m.Genre{}, "name_slug = ?", genre.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, m.NewBaelorError("genre_already_exists", nil))
		return
	}

	// Insert into database
	genre.Init()
	ctrl.context.Db.Create(&genre)
	if ctrl.context.Db.NewRecord(genre) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_genre", nil))
		return
	}
	c.JSON(http.StatusCreated, genre.Map())
}

// NewGenresController ..
func NewGenresController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(GenresController)
	ctrl.context = c

	r.GET("genres", ctrl.Get)
	r.GET("genres/:id", ctrl.GetByID)
	r.POST("genres", ctrl.Post)
}
