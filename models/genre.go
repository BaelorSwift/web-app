package models

// Genre contains the data of a specific genre
type Genre struct {
	Audit

	Name        string `gorm:"not null" form:"name" json:"name"`
	NameSlug    string `gorm:"not null" form:"nameSlug" json:"nameSlug"`
	Description string `gorm:"not null" form:"description" json:"description"`
}
