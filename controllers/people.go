package controllers

import (
	"fmt"
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// PeopleController ..
type PeopleController struct {
	context    *models.Context
	identTypes map[string]string
}

const peopleSafeName = "people"

// Get ..
func (ctrl PeopleController) Get(c *gin.Context) {
	var people []models.Person
	start, count := helpers.FindWithPagination(ctrl.context.Db, &people, c, peopleSafeName)
	response := make([]*models.PersonResponse, len(people))
	for i, person := range people {
		response[i] = person.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, peopleSafeName, start, count))
}

// GetByIdent ..
func (ctrl PeopleController) GetByIdent(c *gin.Context) {
	var person models.Person
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)

	if ctrl.context.Db.First(&person, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("person_not_found", nil))
	} else {
		c.JSON(http.StatusOK, person.Map())
	}
}

// Post ..
func (ctrl PeopleController) Post(c *gin.Context) {
	// Validate Payload
	var person models.Person
	status, err := helpers.ValidateJSON(c, &person, peopleSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check person is unique
	person.NameSlug = helpers.GenerateSlug(person.Name)
	if !ctrl.context.Db.First(&models.Person{}, "name_slug = ?", person.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("person_already_exists", nil))
		return
	}

	// Insert into database
	person.Init()
	if ctrl.context.Db.Create(&person); ctrl.context.Db.NewRecord(person) {
		c.JSON(http.StatusInternalServerError,
			models.NewBaelorError("unknown_error_creating_person", nil))
		return
	}
	c.JSON(http.StatusCreated, person.Map())
}

// Delete ..
func (ctrl PeopleController) Delete(c *gin.Context) {
	var person models.Person
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)
	if ctrl.context.Db.First(&person, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("person_not_found", nil))
		return
	}

	errs := ctrl.context.Db.Delete(&person).GetErrors()
	if len(errs) == 0 {
		c.Status(http.StatusNoContent)
	} else {
		ctrl.context.Raven.CaptureError(errs[0], nil)
		c.JSON(http.StatusInternalServerError, models.NewBaelorError("unknown_error_deleting_person", nil))
	}
}

// NewPeopleController ..
func NewPeopleController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(PeopleController)
	ctrl.context = c
	ctrl.identTypes = map[string]string{
		"id":   "id",
		"slug": "name_slug",
	}

	r.GET("people", ctrl.Get)
	r.GET("people/:ident", ctrl.GetByIdent)
	r.POST("people", middleware.BearerAuth(c), ctrl.Post)
	r.DELETE("people/:ident", middleware.BearerAuth(c), ctrl.Delete)
}
