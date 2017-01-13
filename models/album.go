package models

// Album contains the data of a specific album
type Album struct {
	Audit

	Title     string `gorm:"not null" json:"title"`
	TitleSlug string `gorm:"not null" json:"titleSlug"`
	Length    uint64 `gorm:"not null" json:"length"`

	Genres    []Genre  `gorm:"many2many:genres;"    json:"genres"`
	Producers []Person `gorm:"many2many:producers;" json:"producers"`
	LabelID   string
}
