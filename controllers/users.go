package controllers

import (
	"net/http"

	h "github.com/baelorswift/api/helpers"
	m "github.com/baelorswift/api/models"
	"github.com/jmcvetta/randutil"
	"golang.org/x/crypto/bcrypt"
	"gopkg.in/gin-gonic/gin.v1"
)

// UsersController ..
type UsersController struct {
	context *m.Context
}

const userSafeName = "users"

// Get ..
func (ctrl UsersController) Get(c *gin.Context) {
	var users []m.User
	ctrl.context.Db.Find(&users)
	response := make([]*m.UserResponse, len(users))
	for i, user := range users {
		response[i] = user.Map()
	}

	c.JSON(http.StatusOK, &response)
}

// GetByID ..
func (ctrl UsersController) GetByID(c *gin.Context) {
	var user m.User
	if ctrl.context.Db.First(&user, "id = ?", c.Param("id")).RecordNotFound() {
		c.JSON(http.StatusNotFound, m.NewBaelorError("user_not_found", nil))
	} else {
		c.JSON(http.StatusOK, user.Map())
	}
}

// Post ..
func (ctrl UsersController) Post(c *gin.Context) {
	// Validate Payload
	var user m.User
	status, err := h.ValidateJSON(c, &user, userSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check album is unique
	if !ctrl.context.Db.First(&m.User{}, "email_address = ?", &user.EmailAddress).RecordNotFound() {
		c.JSON(http.StatusConflict, m.NewBaelorError("email_address_already_exists", nil))
		return
	}

	// Hash Password
	password, bcryptErr := bcrypt.GenerateFromPassword([]byte(user.Password), 12)
	if bcryptErr != nil {
		c.JSON(http.StatusInternalServerError, m.NewBaelorError("error_hashing_password", nil))
		return
	}
	user.Password = string(password[:])

	// Generate API Key
	apiKey, _ := randutil.AlphaString(64)
	user.APIKey = apiKey

	// Insert user into database
	user.Init()
	if ctrl.context.Db.Create(&user); ctrl.context.Db.NewRecord(user) {
		c.JSON(http.StatusInternalServerError,
			m.NewBaelorError("unknown_error_creating_user", nil))
		return
	}

	c.JSON(http.StatusCreated, user.Map())
}

// NewUsersController ..
func NewUsersController(r *gin.RouterGroup, c *m.Context) {
	ctrl := new(UsersController)
	ctrl.context = c

	r.GET("users", ctrl.Get)
	r.GET("users/:id", ctrl.GetByID)
	r.POST("users", ctrl.Post)
}
