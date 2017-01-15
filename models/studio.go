package models

// Studio ..
type Studio struct {
	Audit

	Name      string `gorm:"not null" json:"name"`
	NameSlug  string `gorm:"not null" json:"nameSlug"`
	FoundedIn string `gorm:"not null" json:"foundedIn"`
	FoundedAt int64  `gorm:"not null" json:"foundedAt"`
	Website   string `gorm:"not null" json:"website"`

	Albums []Album `json:"albums"`
}

// StudioResponse ..
type StudioResponse struct {
	Audit

	Name      string `json:"name"`
	NameSlug  string `json:"nameSlug"`
	FoundedIn string `json:"foundedIn"`
	FoundedAt int64  `json:"foundedAt"`
	Website   string `json:"website"`

	Albums []Album `json:"albums,omitempty"`
}

// Map ..
func (studio Studio) Map() StudioResponse {
	return StudioResponse{
		Audit: studio.Audit,

		Name:      studio.Name,
		NameSlug:  studio.NameSlug,
		FoundedIn: studio.FoundedIn,
		FoundedAt: studio.FoundedAt,
		Website:   studio.Website,
		Albums:    studio.Albums,
	}
}
