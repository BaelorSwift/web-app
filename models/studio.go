package models

// Studio ..
type Studio struct {
	Audit

	Name      string `gorm:"not null" json:"name"`
	NameSlug  string `gorm:"not null" json:"nameSlug"`
	FoundedIn string `gorm:"not null" json:"foundedIn"`
	FoundedAt int64  `gorm:"not null" json:"foundedAt"`
	Website   string `gorm:"not null" json:"website"`

	Albums []Album
}
