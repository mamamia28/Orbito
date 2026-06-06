FROM nginx:alpine

ARG PACKAGE_URL

RUN wget "${PACKAGE_URL}" -O /usr/share/nginx/html/orbito.zip

COPY index.html /usr/share/nginx/html/index.html