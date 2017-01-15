package models

// Person ..
type Person struct {
	Audit

	Name        string `gorm:"not null" json:"name"`
	NameSlug    string `gorm:"not null" json:"name_slug"`
	Nationality string `gorm:"not null" json:"nationality"`
	Occupation  string `gorm:"not null" json:"occupation"`
}

// PersonResponse ..
type PersonResponse struct {
	Audit

	Name        string `json:"name"`
	NameSlug    string `json:"name_slug"`
	Nationality string `json:"nationality"`
	Occupation  string `json:"occupation"`
}

// TableName ..
func (Person) TableName() string {
	return "people"
}

// Map ..
func (person Person) Map() PersonResponse {
	return PersonResponse{
		Audit: person.Audit,

		Name:        person.Name,
		NameSlug:    person.NameSlug,
		Nationality: person.Nationality,
		Occupation:  person.Occupation,
	}
}
