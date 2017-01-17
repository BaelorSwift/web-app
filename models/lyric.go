package models

// Lyric ..
type Lyric struct {
	Audit

	Index          uint   `gorm:"not null"                   json:"index"`
	Content        string `gorm:"not null"                   json:"content"`
	SongID         string `gorm:"not null"                   json:"song_id"`
	IsStructureGap bool   `gorm:"not null"                   json:"is_structure_gap"`
	Song           Song   `gorm:"not null;ForeignKey:SongID" json:"song"`
}

// LyricResponse ..
type LyricResponse struct {
	Audit

	Index          uint          `json:"index"`
	Content        string        `json:"content"`
	IsStructureGap bool          `json:"is_structure_gap"`
	Song           *SongResponse `json:"song,omitempty"`
}

// Map ..
func (lyric Lyric) Map() *LyricResponse {
	if lyric.ID == "" {
		return nil
	}

	return &LyricResponse{
		Audit: lyric.Audit,

		Index:          lyric.Index,
		Content:        lyric.Content,
		IsStructureGap: lyric.IsStructureGap,
		Song:           lyric.Song.Map(),
	}
}
