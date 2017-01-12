package controllers

import (
	"net/http"
	"time"

	"fmt"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	s "github.com/baelorswift/api/services"
	uuid "github.com/satori/go.uuid"
	"gopkg.in/gin-gonic/gin.v1"
)

// GenresController ..
type GenresController struct{}

const genreSafeName = "genres"

// GenresGet ..
func (ctrl GenresController) GenresGet(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	var genres []m.Genre
	svc.Db.Find(&genres)

	c.JSON(http.StatusOK, genres)
}

// GenresPost ...
func (ctrl GenresController) GenresPost(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	var genre m.Genre

	// Validate Payload
	if c.BindJSON(&genre) != nil {
		c.JSON(http.StatusUnprocessableEntity,
			m.NewBaelorError("invalid_json", map[string][]string{}))
		return
	}

	// Validate JSON
	fmt.Println(genre)
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

	// Set metadata fields
	now := time.Now().UTC()
	genre.ID = uuid.NewV4().String()
	genre.CreatedAt = now
	genre.UpdatedAt = now

	// Insert into database
	svc.Db.Create(&genre)
	if svc.Db.NewRecord(genre) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_genre", map[string][]string{}))
		return
	}
	c.JSON(http.StatusCreated, genre)
}

// NewGenresController ..
func NewGenresController(r *gin.RouterGroup) {
	ctrl := new(GenresController)

	r.GET("genres", ctrl.GenresGet)
	r.POST("genres", ctrl.GenresPost)
}
