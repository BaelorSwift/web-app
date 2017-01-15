package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// LabelsController ..
type LabelsController struct {
	context *m.Context
}

const labelsSafeName = "labels"

// Get ..
func (ctrl LabelsController) Get(c *gin.Context) {
	var labels []m.Label
	ctrl.context.Db.Find(&labels)
	response := make([]m.LabelResponse, len(labels))
	for i, label := range labels {
		response[i] = label.Map()
	}
	c.JSON(http.StatusOK, &response)
}

// GetByID ..
func (ctrl LabelsController) GetByID(c *gin.Context) {
	var label m.Label
	if ctrl.context.Db.First(&label, "id = ?", c.Param("id")).RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("label_not_found", nil))
	} else {
		c.JSON(http.StatusOK, label.Map())
	}
}

// Post ..
func (ctrl LabelsController) Post(c *gin.Context) {
	// Validate Payload
	var label m.Label
	status, err := h.ValidateJSON(c, &label, labelsSafeName)
	if err != nil {
		c.JSON(status, &err)
	}

	// Check label is unique
	label.NameSlug = h.GenerateSlug(label.Name)
	if !ctrl.context.Db.First(&m.Label{}, "name_slug = ?", &label.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, m.NewBaelorError("label_already_exists", nil))
		return
	}

	// Insert into database
	label.Init()
	label.NameSlug = h.GenerateSlug(label.Name)
	ctrl.context.Db.Create(&label)
	if ctrl.context.Db.NewRecord(label) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_label", nil))
		return
	}
	c.JSON(http.StatusCreated, label.Map())
}

// NewLabelsController ..
func NewLabelsController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(LabelsController)
	ctrl.context = c

	r.GET("labels", ctrl.Get)
	r.GET("labels/:id", ctrl.GetByID)
	r.POST("labels", ctrl.Post)
}
