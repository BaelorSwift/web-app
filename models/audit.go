package models

import "time"

// Audit contains all the base audit info of a model
type Audit struct {
	ID        string     `gorm:"primary_key"                                   form:"id"        json:"id"`
	CreatedAt time.Time  `gorm:"not null"    sql:"DEFAULT:current_timestamp()" form:"createdAt" json:"createdAt"`
	UpdatedAt time.Time  `gorm:"not null"    sql:"DEFAULT:current_timestamp()" form:"updatedAt" json:"updatedAt"`
	DeletedAt *time.Time `                                                     form:"deletedAt" json:"deletedAt"`
}
