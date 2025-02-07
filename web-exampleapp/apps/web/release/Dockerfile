FROM nginx:alpine

WORKDIR /app
COPY apps/web/release/config_and_run.sh .
COPY apps/web/release/nginx.conf .

COPY dist/apps/web /usr/share/nginx/html

ARG APP_VERSION
ENV NX_APP_VERSION ${APP_VERSION}

ENTRYPOINT envsubst '$ROOT_DOMAIN' < /app/nginx.conf > /etc/nginx/conf.d/default.conf && \
    /app/config_and_run.sh
