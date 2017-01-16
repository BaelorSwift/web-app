package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	"gopkg.in/gin-gonic/gin.v1"
)

// PeopleController ..
type PeopleController struct {
	context *m.Context
}

const peopleSafeName = "people"

// Get ..
func (ctrl PeopleController) Get(c *gin.Context) {
	var people []m.Person
	ctrl.context.Db.Find(&people)
	response := make([]*m.PersonResponse, len(people))
	for i, person := range people {
		response[i] = person.Map()
	}
	c.JSON(http.StatusOK, response)
}

// GetByID ..
func (ctrl PeopleController) GetByID(c *gin.Context) {
	var person m.Person
	if ctrl.context.Db.First(&person, "id = ?", c.Param("id")).RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("person_not_found", nil))
	} else {
		c.JSON(http.StatusOK, person.Map())
	}
}

// Post ..
func (ctrl PeopleController) Post(c *gin.Context) {
	// Validate Payload
	var person m.Person
	status, err := h.ValidateJSON(c, &person, peopleSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check person is unique
	person.NameSlug = h.GenerateSlug(person.Name)
	if !ctrl.context.Db.First(&m.Person{}, "name_slug = ?", person.NameSlug).RecordNotFound() {
		c.JSON(http.StatusConflict, m.NewBaelorError("person_already_exists", nil))
		return
	}

	// Insert into database
	person.Init()
	if ctrl.context.Db.Create(&person); ctrl.context.Db.NewRecord(person) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_person", nil))
		return
	}
	c.JSON(http.StatusCreated, person.Map())
}

// NewPeopleController ..
func NewPeopleController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(PeopleController)
	ctrl.context = c

	r.GET("people", ctrl.Get)
	r.GET("people/:id", ctrl.GetByID)
	r.POST("people", ctrl.Post)
}
