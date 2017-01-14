package models

// Album contains the data of a specific album
type Album struct {
	Audit

	Title       string `gorm:"not null" json:"title"`
	TitleSlug   string `gorm:"not null" json:"titleSlug"`
	Description string `gorm:"not null" json:"description"`
	Length      uint64 `gorm:"not null" json:"length"`
	RecordedAt  string `gorm:"not null" json:"recordedAt"`
	ReleasedAt  uint64 `gorm:"not null" json:"releasedAt"`
	Studio      string `gorm:"not null" json:"studio"`
	CoverImage  string `gorm:"not null" json:"coverImage"`

	Genres    []Genre  `gorm:"many2many:genres;"    json:"genres"`
	Producers []Person `gorm:"many2many:producers;" json:"producers"`
	Studios   []Studio `gorm:"many2many:studios;"   json:"studios"`
	LabelID   string   `gorm:"index"                json:"labelId"`
	Label     Label
}
