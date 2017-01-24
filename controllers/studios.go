package controllers

import (
	"fmt"
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"

	"gopkg.in/gin-gonic/gin.v1"
)

// StudiosController ..
type StudiosController struct {
	context *models.Context
}

const studioSafeName = "studios"

// Get ..
func (ctrl StudiosController) Get(c *gin.Context) {
	var studios []models.Studio
	start, count := helpers.FindWithPagination(ctrl.context.Db, &studios, c, songSafeName)
	response := make([]*models.StudioResponse, len(studios))
	for i, studio := range studios {
		response[i] = studio.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, studioSafeName, start, count))
}

// GetByIdent ..
func (ctrl StudiosController) GetByIdent(c *gin.Context) {
	var studio models.Studio
	identType, ident := helpers.DetectParamType(c.Param("ident"), "name")

	if ctrl.context.Db.First(&studio, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("studio_not_found", nil))
	} else {
		c.JSON(http.StatusOK, studio.Map())
	}
}

// Post ..
func (ctrl StudiosController) Post(c *gin.Context) {
	// Validate Payload
	var studio models.Studio
	status, err := helpers.ValidateJSON(c, &studio, studioSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check studio is unique
	studio.NameSlug = helpers.GenerateSlug(studio.Name)
	if !ctrl.context.Db.First(&models.Studio{}, "name_slug = ?", studio.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("studio_already_exists", nil))
		return
	}

	// Insert into database
	studio.Init()
	if ctrl.context.Db.Create(&studio); ctrl.context.Db.NewRecord(studio) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_studio", nil))
		return
	}

	c.JSON(http.StatusCreated, studio.Map())
}

// NewStudiosController ..
func NewStudiosController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(StudiosController)
	ctrl.context = c

	r.GET("studios", ctrl.Get)
	r.GET("studios/:ident", ctrl.GetByIdent)
	r.POST("studios", middleware.BearerAuth(c), ctrl.Post)
}
