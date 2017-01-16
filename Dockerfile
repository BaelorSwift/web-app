FROM golang:1.4.2

# Create a directory inside the container to store all our application and then make it the working directory.
RUN mkdir -p /go/src/github.com/baelorswift/api
WORKDIR /go/src/github.com/baelorswift/api
COPY . /go/src/github.com/baelorswift/api

RUN go get gopkg.in/gin-gonic/gin.v1
RUN go-wrapper download
RUN go-wrapper install

ENV CONFIGOR_ENV_PREFIX BAE
ENV BAE_ADDRESS ":3000"
ENV BAE_DSN "https://5f4087eadcf04e9daf4afc35a519f60a@sentry.io/103366"

EXPOSE 3000

CMD gin run
