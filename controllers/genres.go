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

// GetByID ..
func (GenresController) GetByID(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	var genre m.Genre
	if svc.Db.First(&genre, "id = ?", c.Param("id")); genre.Name != "" {
		c.JSON(http.StatusOK, &genre)
	} else {
		c.JSON(http.StatusNotFound, m.NewBaelorError("genre_not_found", nil))
	}
}

// Get ..
func (GenresController) Get(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	var genres []m.Genre
	svc.Db.Find(&genres)
	c.JSON(http.StatusOK, &genres)
}

// Post ...
func (GenresController) Post(c *gin.Context) {
	svc := s.NewDatabaseService(genreSafeName)
	defer svc.Close()

	// Validate Payload
	var genre m.Genre
	status, err := h.ValidateJSON(c, &genre, genreSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check genre is unique
	if svc.Exists("name_slug = ?", &genre.NameSlug) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("genre_already_exists", nil))
		return
	}

	// Insert into database
	genre.Init()
	svc.Db.Create(&genre)
	if svc.Db.NewRecord(genre) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_genre", nil))
		return
	}
	c.JSON(http.StatusCreated, &genre)
}

// NewGenresController ..
func NewGenresController(r *gin.RouterGroup) {
	ctrl := new(GenresController)

	r.GET("genres", ctrl.Get)
	r.GET("genres/:id", ctrl.GetByID)
	r.POST("genres", ctrl.Post)
}
