package models

// Label ..
type Label struct {
	Audit

	Name      string `gorm:"not null" json:"name"`
	NameSlug  string `gorm:"not null" json:"nameSlug"`
	FoundedAt uint64 `gorm:"not null" json:"founded"`
	FoundedIn string `gorm:"not null" json:"foundedIn"`
	Location  string `gorm:"not null" json:"location"`
	Website   string `gorm:"not null" json:"website"`

	Albums []Album `gorm:"ForeignKey:LabelID" json:"albums"`
}

// LabelResponse ..
type LabelResponse struct {
	Audit

	Name      string  `json:"name"`
	NameSlug  string  `json:"nameSlug"`
	FoundedAt uint64  `json:"founded"`
	FoundedIn string  `json:"foundedIn"`
	Location  string  `json:"location"`
	Website   string  `json:"website"`
	Albums    []Album `json:"albums"`
}

// Map ..
func (label Label) Map() LabelResponse {
	return LabelResponse{
		Audit: label.Audit,

		Name:      label.Name,
		NameSlug:  label.NameSlug,
		FoundedAt: label.FoundedAt,
		FoundedIn: label.FoundedIn,
		Location:  label.Location,
		Website:   label.Website,
		Albums:    label.Albums,
	}
}
