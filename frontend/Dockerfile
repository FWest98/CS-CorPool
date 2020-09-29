# build stage
FROM node:lts-alpine as build
WORKDIR /app
# ENV VUE_APP_BACKEND_SERVER=$BACKEND_SERVER
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# COMMAND echo $VUE_APP_BACKEND_SERVER
# COMMAND echo $BACKEND_SERVER

# production stage
FROM nginx:stable-alpine as final
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]