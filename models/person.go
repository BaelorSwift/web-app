package models

// Person ..
type Person struct {
	Audit

	Name        string `gorm:"not null" json:"name"`
	NameSlug    string `gorm:"not null" json:"nameSlug"`
	Nationality string `gorm:"not null" json:"nationality"`
	Occupation  string `gorm:"not null" json:"occupation"`
}

// TableName ..
func (Person) TableName() string {
	return "people"
}
