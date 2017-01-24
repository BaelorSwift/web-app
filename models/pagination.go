package models

import (
	"fmt"
)

const pagingFormat = "/v1/%s?start=%d&count=%d"

// PaginationResponse ..
type PaginationResponse struct {
	Data  interface{}       `json:"data"`
	Links map[string]string `json:"links"`
}

// NewPaginationResponse ..
func NewPaginationResponse(data interface{}, safeName string, start, count int64) *PaginationResponse {
	links := map[string]string{
		"first": fmt.Sprintf(pagingFormat, safeName, 0, count),
		"next":  fmt.Sprintf(pagingFormat, safeName, start+count, count),
	}

	// Check if showing previous will clip into negative on the start index - default to 0
	if start-count > 0 {
		links["previous"] = fmt.Sprintf(pagingFormat, safeName, start-count, count)
	} else {
		links["previous"] = fmt.Sprintf(pagingFormat, safeName, 0, count)
	}

	return &PaginationResponse{
		Data:  data,
		Links: links,
	}
}
