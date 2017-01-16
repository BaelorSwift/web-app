package models

// Label ..
type Label struct {
	Audit

	Name      string `gorm:"not null"              json:"name"`
	NameSlug  string `gorm:"not null;unique_index" json:"name_slug"`
	FoundedAt uint64 `gorm:"not null"              json:"founded_at"`
	FoundedIn string `gorm:"not null"              json:"founded_in"`
	Location  string `gorm:"not null"              json:"location"`
	Website   string `gorm:"not null"              json:"website"`

	Albums []Album `gorm:"ForeignKey:LabelID" json:"albums"`
}

// LabelResponse ..
type LabelResponse struct {
	Audit

	Name      string           `json:"name"`
	NameSlug  string           `json:"name_slug"`
	FoundedAt uint64           `json:"founded_at"`
	FoundedIn string           `json:"founded_in"`
	Location  string           `json:"location"`
	Website   string           `json:"website"`
	Albums    []*AlbumResponse `json:"albums"`
}

// Map ..
func (label Label) Map() *LabelResponse {
	if label.ID == "" {
		return nil
	}

	lbl := LabelResponse{
		Audit: label.Audit,

		Name:      label.Name,
		NameSlug:  label.NameSlug,
		FoundedAt: label.FoundedAt,
		FoundedIn: label.FoundedIn,
		Location:  label.Location,
		Website:   label.Website,
		Albums:    make([]*AlbumResponse, len(label.Albums)),
	}

	for i, album := range label.Albums {
		lbl.Albums[i] = album.Map()
	}

	return &lbl
}
