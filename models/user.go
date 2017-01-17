package models

// User ..
type User struct {
	Audit

	Name         string `gorm:"not null"              json:"name"`
	EmailAddress string `gorm:"not null;unique_index" json:"email_address"`
	Password     string `gorm:"not null"              json:"password"`
	APIKey       string `gorm:"not null;unique_index" json:"api_key"`
}

// UserResponse ..
type UserResponse struct {
	Audit

	Name         string `json:"name"`
	EmailAddress string `json:"email_address"`
	APIKey       string `json:"api_key"`
}

// Map ..
func (user User) Map() *UserResponse {
	if user.ID == "" {
		return nil
	}

	return &UserResponse{
		Audit: user.Audit,

		Name:         user.Name,
		EmailAddress: user.EmailAddress,
		APIKey:       user.APIKey,
	}
}
