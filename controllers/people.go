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

// GetByID ..
func (PeopleController) GetByID(c *gin.Context) {
	svc := s.NewDatabaseService(peopleSafeName)
	defer svc.Close()

	var person m.Person
	if svc.Db.First(&person, "id = ?", c.Param("id")); person.Name != "" {
		c.JSON(http.StatusOK, &person)
	} else {
		c.JSON(http.StatusNotFound, m.NewBaelorError("person_not_found", nil))
	}
}

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
	status, err := h.ValidateJSON(c, &person, peopleSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check person is unique
	if svc.Exists("name_slug = ?", &person.NameSlug) {
		c.JSON(http.StatusConflict,
			m.NewBaelorError("person_already_exists", nil))
		return
	}

	// Insert into database
	person.Init()
	if svc.Db.Create(&person); svc.Db.NewRecord(person) {
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
	r.GET("people/:id", ctrl.GetByID)
	r.POST("people", ctrl.Post)
}
