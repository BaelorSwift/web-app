package models

// Analytic holds the database model for API analytics
type Analytic struct {
	Audit

	RequestID          string `gorm:"not null;unique_index" json:""`
	RequestMethod      string `gorm:"not null"              json:"-"`
	RequestIP          string `gorm:"not null"              json:"-"`
	RequestPayloadSize int64  `gorm:"not null"              json:"-"`
	RequestPath        string `gorm:"not null"              json:"-"`

	ResponseStatus      int `gorm:"not null" json:"-"`
	ResponsePayloadSize int `gorm:"not null" json:"-"`
}

// AnalyticResponse is the model marshaled into json and returned to the user
type AnalyticResponse struct {
	Request  *analyticRequestBody  `json:"request"`
	Response *analyticResponseBody `json:"response"`
}

type analyticRequestBody struct {
	ID          string `json:"id"`
	Method      string `json:"method"`
	IP          string `json:"ip"`
	PayloadSize int64  `json:"payload_size"`
	Path        string `json:"path"`
}
type analyticResponseBody struct {
	Status      int `json:"status"`
	PayloadSize int `json:"payload_size"`
}

// Map converts a Analytic struct into a pointer to a AnalyticResponse struct, ready for
// marshalling into JSON
func (analytic Analytic) Map() *AnalyticResponse {
	return &AnalyticResponse{
		Request: &analyticRequestBody{
			ID:          analytic.RequestID,
			Method:      analytic.RequestMethod,
			IP:          analytic.RequestIP,
			PayloadSize: analytic.RequestPayloadSize,
			Path:        analytic.RequestPath,
		},
		Response: &analyticResponseBody{
			Status:      analytic.ResponseStatus,
			PayloadSize: analytic.ResponsePayloadSize,
		},
	}
}
