FROM golang:1.7.4

# Create a directory inside the container to store all our application and then make it the working directory.
RUN mkdir -p /go/src/github.com/baelorswift/api
WORKDIR /go/src/github.com/baelorswift/api
COPY . /go/src/github.com/baelorswift/api

RUN go get gopkg.in/gin-gonic/gin.v1
RUN go-wrapper download
RUN go-wrapper install

EXPOSE 3000

CMD go run ./server.go
