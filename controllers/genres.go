package controllers

import (
	"fmt"
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// GenresController ..
type GenresController struct {
	context *models.Context
}

const genreSafeName = "genres"

// Get ..
func (ctrl GenresController) Get(c *gin.Context) {
	var genres []models.Genre
	start, count := helpers.FindWithPagination(ctrl.context.Db, &genres, c, genreSafeName)
	response := make([]*models.GenreResponse, len(genres))
	for i, genre := range genres {
		response[i] = genre.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, genreSafeName, start, count))
}

// GetByIdent ..
func (ctrl GenresController) GetByIdent(c *gin.Context) {
	var genre models.Genre
	identType, ident := helpers.DetectParamType(c.Param("ident"), "name")

	if ctrl.context.Db.First(&genre, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("genre_not_found", nil))
	} else {
		c.JSON(http.StatusOK, genre.Map())
	}
}

// Post ...
func (ctrl GenresController) Post(c *gin.Context) {
	// Validate Payload
	var genre models.Genre
	status, err := helpers.ValidateJSON(c, &genre, genreSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check genre is unique
	genre.NameSlug = helpers.GenerateSlug(genre.Name)
	if !ctrl.context.Db.First(&models.Genre{}, "name_slug = ?", genre.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("genre_already_exists", nil))
		return
	}

	// Insert into database
	genre.Init()
	ctrl.context.Db.Create(&genre)
	if ctrl.context.Db.NewRecord(genre) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_genre", nil))
		return
	}
	c.JSON(http.StatusCreated, genre.Map())
}

// NewGenresController ..
func NewGenresController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(GenresController)
	ctrl.context = c

	r.GET("genres", ctrl.Get)
	r.GET("genres/:ident", ctrl.GetByIdent)
	r.POST("genres", middleware.BearerAuth(c), ctrl.Post)
}
