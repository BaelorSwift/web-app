package models

import (
	"time"
)

// Label ..
type Label struct {
	Audit

	Name      string    `gorm:"not null" json:"name"`
	NameSlug  string    `gorm:"not null" json:"nameSlug"`
	Founded   time.Time `gorm:"not null" json:"founded"`
	FoundedIn string    `gorm:"not null" json:"foundedIn"`
	Location  string    `gorm:"not null" json:"location"`
	Website   string    `gorm:"not null" json:"website"`
}
