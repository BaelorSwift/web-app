package models

// Album contains the data of a specific album
type Album struct {
	Audit

	Title       string `gorm:"not null"                    json:"title"`
	TitleSlug   string `gorm:"not null;unique_index"       json:"title_slug"`
	Description string `gorm:"not null;type:varchar(1500)" json:"description"`
	Length      uint64 `gorm:"not null"                    json:"length"`
	RecordedAt  string `gorm:"not null"                    json:"recorded_at"`
	ReleasedAt  uint64 `gorm:"not null"                    json:"released_at"`
	Studio      string `gorm:"not null"                    json:"studio"`
	CoverImage  string `gorm:"not null"                    json:"cover_image"`

	Songs     []Song   `gorm:"ForeignKey:AlbumID"         json:"albums"`
	Genres    []Genre  `gorm:"many2many:album_genres;"    json:"genres"`
	Producers []Person `gorm:"many2many:album_producers;" json:"producers"`
	Studios   []Studio `gorm:"many2many:album_studios;"   json:"studios"`
	LabelID   string   `gorm:"not null;index"             json:"label_id"`
	Label     Label    `                                  json:"label"`

	ProducerIDs []string `gorm:"-" json:"producer_ids"`
	GenreIDs    []string `gorm:"-" json:"genre_ids"`
	StudioIDs   []string `gorm:"-" json:"studio_ids"`
}

// AlbumResponse ..
type AlbumResponse struct {
	Audit

	Title       string `json:"title"`
	TitleSlug   string `json:"title_slug"`
	Description string `json:"description"`
	Length      uint64 `json:"length"`
	RecordedAt  string `json:"recorded_at"`
	ReleasedAt  uint64 `json:"released_at"`
	CoverImage  string `json:"cover_image"`

	Songs     []*SongResponse   `json:"songs"`
	Genres    []*GenreResponse  `json:"genres"`
	Producers []*PersonResponse `json:"producers"`
	Studios   []*StudioResponse `json:"studios"`
	Label     *LabelResponse    `json:"label,omitempty"`
}

// Map ..
func (album Album) Map() *AlbumResponse {
	if album.ID == "" {
		return nil
	}

	albm := AlbumResponse{
		Audit: album.Audit,

		Title:       album.Title,
		TitleSlug:   album.TitleSlug,
		Description: album.Description,
		Length:      album.Length,
		RecordedAt:  album.RecordedAt,
		ReleasedAt:  album.ReleasedAt,
		CoverImage:  album.CoverImage,

		Songs:     make([]*SongResponse, len(album.Songs)),
		Genres:    make([]*GenreResponse, len(album.Genres)),
		Producers: make([]*PersonResponse, len(album.Producers)),
		Studios:   make([]*StudioResponse, len(album.Studios)),
		Label:     album.Label.Map(),
	}

	for i, song := range album.Songs {
		albm.Songs[i] = song.Map()
	}
	for i, genre := range album.Genres {
		albm.Genres[i] = genre.Map()
	}
	for i, producer := range album.Producers {
		albm.Producers[i] = producer.Map()
	}
	for i, studio := range album.Studios {
		albm.Studios[i] = studio.Map()
	}

	return &albm
}
