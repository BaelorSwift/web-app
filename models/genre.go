package models

// Genre contains the data of a specific genre
type Genre struct {
	Audit

	Name        string `json:"name"`
	NameSlug    string `json:"nameSlug"`
	Description string `json:"description"`
}
