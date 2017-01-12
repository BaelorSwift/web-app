package models

import "github.com/howeyc/crc16"

// BaelorError contains error information to be picked up by the custom error
// middleware for gin
type BaelorError struct {
	Code     string              `json:"code"`
	Status   uint16              `json:"status"`
	Metadata map[string][]string `json:"metadata"`
}

// NewBaelorError creates a new BaelorError with metadata
func NewBaelorError(code string, metadata map[string][]string) BaelorError {
	return BaelorError{
		Code:     code,
		Status:   crc16.ChecksumCCITT([]byte(code)),
		Metadata: metadata,
	}
}
