package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	s "github.com/baelorswift/api/services"
	"gopkg.in/gin-gonic/gin.v1"
)

// GenresController ..
type GenresController struct{}

const genreSafeName = "genres"

// Get ..
func (GenresController) Get(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	var genres []m.Genre
	svc.Db.Find(&genres)

	c.JSON(http.StatusOK, genres)
}

// Post ...
func (GenresController) Post(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	var genre m.Genre

	// Validate Payload
	if c.BindJSON(&genre) != nil {
		c.JSON(http.StatusBadRequest,
			m.NewBaelorError("invalid_json", map[string][]string{}))
		return
	}

	// Validate JSON
	valid, err := h.Validate(&genre, genreSafeName)
	if !valid {
		c.JSON(http.StatusUnprocessableEntity,
			m.NewBaelorError("validation_failed", err))
		return
	}

	// Check genre is unique
	if svc.Exists("name_slug = ?", genre.NameSlug) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("genre_already_exists", map[string][]string{}))
		return
	}

	// Insert into database
	genre.Init()
	svc.Db.Create(&genre)
	if svc.Db.NewRecord(genre) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_genre", map[string][]string{}))
		return
	}
	c.JSON(http.StatusCreated, &genre)
}

// NewGenresController ..
func NewGenresController(r *gin.RouterGroup) {
	ctrl := new(GenresController)

	r.GET("genres", ctrl.Get)
	r.POST("genres", ctrl.Post)
}
