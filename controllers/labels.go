package controllers

import (
	"fmt"
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// LabelsController ..
type LabelsController struct {
	context    *models.Context
	identTypes map[string]string
}

const labelsSafeName = "labels"

// Get ..
func (ctrl LabelsController) Get(c *gin.Context) {
	var labels []models.Label
	start, count := helpers.FindWithPagination(ctrl.context.Db, &labels, c, labelsSafeName)
	response := make([]*models.LabelResponse, len(labels))
	for i, label := range labels {
		response[i] = label.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, labelsSafeName, start, count))
}

// GetByIdent ..
func (ctrl LabelsController) GetByIdent(c *gin.Context) {
	var label models.Label
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)

	if ctrl.context.Db.First(&label, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("label_not_found", nil))
	} else {
		c.JSON(http.StatusOK, label.Map())
	}
}

// Post ..
func (ctrl LabelsController) Post(c *gin.Context) {
	// Validate Payload
	var label models.Label
	status, err := helpers.ValidateJSON(c, &label, labelsSafeName)
	if err != nil {
		c.JSON(status, &err)
	}

	// Check label is unique
	label.NameSlug = helpers.GenerateSlug(label.Name)
	if !ctrl.context.Db.First(&models.Label{}, "name_slug = ?", &label.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("label_already_exists", nil))
		return
	}

	// Insert into database
	label.Init()
	label.NameSlug = helpers.GenerateSlug(label.Name)
	ctrl.context.Db.Create(&label)
	if ctrl.context.Db.NewRecord(label) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_label", nil))
		return
	}
	c.JSON(http.StatusCreated, label.Map())
}

// Delete ..
func (ctrl LabelsController) Delete(c *gin.Context) {
	var label models.Label
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)
	if ctrl.context.Db.First(&label, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("label_not_found", nil))
		return
	}

	errs := ctrl.context.Db.Delete(&label).GetErrors()
	if len(errs) == 0 {
		c.Status(http.StatusNoContent)
	} else {
		ctrl.context.Raven.CaptureError(errs[0], nil)
		c.JSON(http.StatusInternalServerError, models.NewBaelorError("unknown_error_deleting_label", nil))
	}
}

// NewLabelsController ..
func NewLabelsController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(LabelsController)
	ctrl.context = c
	ctrl.identTypes = map[string]string{
		"id":   "id",
		"slug": "name_slug",
	}

	r.GET("labels", ctrl.Get)
	r.GET("labels/:ident", ctrl.GetByIdent)
	r.POST("labels", middleware.BearerAuth(c), ctrl.Post)
	r.DELETE("labels/:ident", middleware.BearerAuth(c), ctrl.Delete)
}
