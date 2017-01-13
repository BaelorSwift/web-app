package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	s "github.com/baelorswift/api/services"
	"gopkg.in/gin-gonic/gin.v1"
)

// LabelsController ..
type LabelsController struct{}

const labelsSafeName = "labels"

// GetByID ..
func (LabelsController) GetByID(c *gin.Context) {
	svc := s.NewDatabaseService(labelsSafeName)
	defer svc.Close()

	var label m.Label
	if svc.Db.First(&label, "id = ?", c.Param("id")); label.ID != "" {
		c.JSON(http.StatusOK, &label)
	} else {
		c.JSON(http.StatusNotFound, m.NewBaelorError("label_not_found", nil))
	}
}

// Get ..
func (LabelsController) Get(c *gin.Context) {
	svc := s.NewDatabaseService(labelsSafeName)
	defer svc.Close()

	var labels []m.Label
	svc.Db.Find(&labels)
	c.JSON(http.StatusOK, &labels)
}

// Post ..
func (LabelsController) Post(c *gin.Context) {
	svc := s.NewDatabaseService(labelsSafeName)
	defer svc.Close()

	// Validate Payload
	var label m.Label
	status, err := h.ValidateJSON(c, &label, labelsSafeName)
	if err != nil {
		c.JSON(status, &err)
	}

	// Check label is unique
	if svc.Exists("name_slug = ?", h.GenerateSlug(label.Name)) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("genre_already_exists", nil))
		return
	}

	// Insert into database
	label.Init()
	label.NameSlug = h.GenerateSlug(label.Name)
	svc.Db.Create(&label)
	if svc.Db.NewRecord(label) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_label", nil))
		return
	}
	c.JSON(http.StatusCreated, &label)
}

// NewLabelsController ..
func NewLabelsController(r *gin.RouterGroup) {
	ctrl := new(LabelsController)

	r.GET("labels", ctrl.Get)
	r.GET("labels/:id", ctrl.GetByID)
	r.POST("labels", ctrl.Post)
}
