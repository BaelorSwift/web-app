package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	s "github.com/baelorswift/api/services"
	"gopkg.in/gin-gonic/gin.v1"
)

// PeopleController ..
type PeopleController struct{}

const peopleSafeName = "people"

// Get ..
func (PeopleController) Get(c *gin.Context) {
	svc := s.NewDatabaseService(peopleSafeName)
	defer svc.Close()

	var people []m.Person
	svc.Db.Find(&people)
	c.JSON(http.StatusOK, &people)
}

// Post ..
func (PeopleController) Post(c *gin.Context) {
	svc := s.NewDatabaseService(peopleSafeName)
	defer svc.Close()

	// Validate Payload
	var person m.Person
	x := c.BindJSON(&person)
	if x != nil {
		c.JSON(http.StatusBadRequest,
			m.NewBaelorError("invalid_json", nil))
		return
	}

	// Validate JSON
	valid, err := h.Validate(&person, peopleSafeName)
	if !valid {
		c.JSON(http.StatusUnprocessableEntity,
			m.NewBaelorError("validation_failed", err))
		return
	}

	// Check genre is unique
	if svc.Exists("name_slug = ?", &person.NameSlug) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("genre_already_exists", map[string][]string{}))
		return
	}

	// Insert into database
	person.Init()
	svc.Db.Create(&person)
	if svc.Db.NewRecord(person) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_person", nil))
		return
	}
	c.JSON(http.StatusCreated, &person)
}

// NewPeopleController ..
func NewPeopleController(r *gin.RouterGroup) {
	ctrl := new(PeopleController)

	r.GET("people", ctrl.Get)
	r.POST("people", ctrl.Post)
}
