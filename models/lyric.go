package models

// Lyric ..
type Lyric struct {
	Audit

	Index   uint   `gorm:"not null"                   json:"index"`
	Content string `gorm:"not null"                   json:"content"`
	SongID  string `gorm:"not null"                   json:"song_id"`
	Song    Song   `gorm:"not null;ForeignKey:SongID" json:"song"`
}

// LyricResponse ..
type LyricResponse struct {
	Audit

	Index   uint          `json:"index"`
	Content string        `json:"content"`
	Song    *SongResponse `json:"song"`
}

// Map ..
func (lyric Lyric) Map() *LyricResponse {
	if lyric.ID == "" {
		return nil
	}

	return &LyricResponse{
		Audit: lyric.Audit,

		Index:   lyric.Index,
		Content: lyric.Content,
		Song:    lyric.Song.Map(),
	}
}
