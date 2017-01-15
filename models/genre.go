package models

// Genre contains the data of a specific genre
type Genre struct {
	Audit

	Name        string `gorm:"not null"                    json:"name"`
	NameSlug    string `gorm:"not null"                    json:"name_slug"`
	Description string `gorm:"not null;type:varchar(1500)" json:"description"`
}

// GenreResponse ..
type GenreResponse struct {
	Audit

	Name        string `json:"name"`
	NameSlug    string `json:"name_slug"`
	Description string `json:"description"`
}

// Map ..
func (genre Genre) Map() GenreResponse {
	return GenreResponse{
		Audit: genre.Audit,

		Name:        genre.Name,
		NameSlug:    genre.NameSlug,
		Description: genre.Description,
	}
}
