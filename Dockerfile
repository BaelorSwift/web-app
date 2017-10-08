FROM node:8
LABEL author="Alex Forbes-Reed"

ENV NODE_ENV="production" \
	PORT="80"

# Move and build the react client code
RUN mkdir -p /usr/local/client
WORKDIR /usr/local/client
COPY client/package.json /usr/local/client
COPY client/package-lock.json /usr/local/client
RUN npm install --production=false --silent
COPY client/ /usr/local/client
RUN npm run build

# Move and build the backend code
RUN mkdir -p /usr/local/backend
WORKDIR /usr/local/backend
COPY backend/package.json /usr/local/backend
COPY backend/package-lock.json /usr/local/backend
RUN npm install --production=false --silent
COPY backend/ /usr/local/backend
RUN npm run build

# Run start command on backend code
EXPOSE 80
CMD ["npm", "start"]
