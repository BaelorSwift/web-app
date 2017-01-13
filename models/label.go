package models

// Label ..l
type Label struct {
	Audit

	Name      string `gorm:"not null" json:"name"`
	NameSlug  string `gorm:"not null" json:"nameSlug"`
	Founded   uint64 `gorm:"not null" json:"founded"`
	FoundedIn string `gorm:"not null" json:"foundedIn"`
	Location  string `gorm:"not null" json:"location"`
	Website   string `gorm:"not null" json:"website"`
}
