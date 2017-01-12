package models

// Person ..
type Person struct {
	Audit

	Name        string `gorm:"not null" json:"name"`
	NameSlug    string `gorm:"not null" json:"nameSlug"`
	Nationality string `gorm:"not null" json:"nationality"`
	DateOfBirth int64  `gorm:"not null" json:"dateOfBirth"`
}

// TableName ..
func (Person) TableName() string {
	return "people"
}
