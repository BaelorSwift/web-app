package models

// Album contains the data of a specific album
type Album struct {
	Audit

	Title     string `gorm:"not null" form:"title"     json:"title"`
	TitleSlug string `gorm:"not null" form:"titleSlug" json:"titleSlug"`
	Length    uint64 `gorm:"not null" form:"length"    json:"length"`
}
