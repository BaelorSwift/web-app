package models

// Album contains the data of a specific album
type Album struct {
	Audit

	Title       string `gorm:"not null"                    json:"title"`
	TitleSlug   string `gorm:"not null"                    json:"titleSlug"`
	Description string `gorm:"not null;type:varchar(1500)" json:"description"`
	Length      uint64 `gorm:"not null"                    json:"length"`
	RecordedAt  string `gorm:"not null"                    json:"recordedAt"`
	ReleasedAt  uint64 `gorm:"not null"                    json:"releasedAt"`
	Studio      string `gorm:"not null"                    json:"studio"`
	CoverImage  string `gorm:"not null"                    json:"coverImage"`

	Genres    []Genre  `gorm:"many2many:album_genres;"    json:"genres"`
	Producers []Person `gorm:"many2many:album_producers;" json:"producers"`
	Studios   []Studio `gorm:"many2many:album_studios;"   json:"studios"`
	LabelID   string   `gorm:"not null;index"             json:"labelId"`
	Label     Label    `                                  json:"label"`

	ProducerIDs []string `gorm:"-" json:"producerIds"`
	GenreIDs    []string `gorm:"-" json:"genreIds"`
	StudioIDs   []string `gorm:"-" json:"studioIds"`
}

// AlbumResponse ..
type AlbumResponse struct {
	Audit

	Title       string `json:"title"`
	TitleSlug   string `json:"titleSlug"`
	Description string `json:"description"`
	Length      uint64 `json:"length"`
	RecordedAt  string `json:"recordedAt"`
	ReleasedAt  uint64 `json:"releasedAt"`
	CoverImage  string `json:"coverImage"`

	Genres    []Genre  `json:"genres"`
	Producers []Person `json:"producers"`
	Studios   []Studio `json:"studios"`
	Label     Label    `json:"label"`
}

// Map ..
func (album Album) Map() AlbumResponse {
	return AlbumResponse{
		Audit: album.Audit,

		Title:       album.Title,
		TitleSlug:   album.TitleSlug,
		Description: album.Description,
		Length:      album.Length,
		RecordedAt:  album.RecordedAt,
		ReleasedAt:  album.ReleasedAt,
		CoverImage:  album.CoverImage,

		Genres:    album.Genres,
		Producers: album.Producers,
		Studios:   album.Studios,
		Label:     album.Label,
	}
}
