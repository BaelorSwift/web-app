package controllers

import (
	"fmt"
	"net/http"

	"github.com/baelorswift/api/helpers"
	"github.com/baelorswift/api/middleware"
	"github.com/baelorswift/api/models"
	"github.com/jmcvetta/randutil"
	"golang.org/x/crypto/bcrypt"
	"gopkg.in/gin-gonic/gin.v1"
)

// UsersController ..
type UsersController struct {
	context    *models.Context
	identTypes map[string]string
}

const userSafeName = "users"

// Get ..
func (ctrl UsersController) Get(c *gin.Context) {
	var users []models.User
	start, count := helpers.FindWithPagination(ctrl.context.Db, &users, c, userSafeName)
	response := make([]*models.UserResponse, len(users))
	for i, user := range users {
		response[i] = user.Map()
	}
	c.JSON(http.StatusOK, models.NewPaginationResponse(&response, userSafeName, start, count))
}

// GetByIdent ..
func (ctrl UsersController) GetByIdent(c *gin.Context) {
	var user models.User
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)

	if ctrl.context.Db.First(&user, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("user_not_found", nil))
	} else {
		c.JSON(http.StatusOK, user.Map())
	}
}

// Post ..
func (ctrl UsersController) Post(c *gin.Context) {
	// Validate Payload
	var user models.User
	status, err := helpers.ValidateJSON(c, &user, userSafeName)
	if err != nil {
		c.JSON(status, &err)
		return
	}

	// Check album is unique
	if !ctrl.context.Db.First(&models.User{}, "email_address = ?", &user.EmailAddress).RecordNotFound() {
		c.JSON(http.StatusConflict, models.NewBaelorError("email_address_already_exists", nil))
		return
	}

	// Hash Password
	password, bcryptErr := bcrypt.GenerateFromPassword([]byte(user.Password), 12)
	if bcryptErr != nil {
		c.JSON(http.StatusInternalServerError, models.NewBaelorError("error_hashing_password", nil))
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
			models.NewBaelorError("unknown_error_creating_user", nil))
		return
	}

	c.JSON(http.StatusCreated, user.Map())
}

// Delete ..
func (ctrl UsersController) Delete(c *gin.Context) {
	var user models.User
	identType, ident := helpers.DetectParamType(c.Param("ident"), ctrl.identTypes)
	if ctrl.context.Db.First(&user, fmt.Sprintf("`%s` = ?", identType), ident).RecordNotFound() {
		c.JSON(http.StatusNotFound, models.NewBaelorError("user_not_found", nil))
		return
	}

	// Validate user id matches the authenticated user id
	if user.ID != c.Get(middleware.AuthUserKey) {
		c.JSON(http.StatusForbidden, models.NewBaelorError("cannot_delete_foreign_user", nil))
	}

	errs := ctrl.context.Db.Delete(&user).GetErrors()
	if len(errs) == 0 {
		c.Status(http.StatusNoContent)
	} else {
		ctrl.context.Raven.CaptureError(errs[0], nil)
		c.JSON(http.StatusInternalServerError, models.NewBaelorError("unknown_error_deleting_user", nil))
	}
}

// NewUsersController ..
func NewUsersController(r *gin.RouterGroup, c *models.Context) {
	ctrl := new(UsersController)
	ctrl.context = c
	ctrl.identTypes = map[string]string{
		"id": "id",
	}

	r.GET("users", middleware.BearerAuth(c), ctrl.Get)
	r.GET("users/:ident", middleware.BearerAuth(c), ctrl.GetByIdent)
	r.POST("users", middleware.BearerAuth(c), ctrl.Post)
	r.DELETE("users/:ident", middleware.BearerAuth(c), ctrl.Delete)
}
