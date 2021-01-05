FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine

# install packages
RUN apk add --no-cache nginx gettext
RUN apk add --no-cache --repository=http://dl-cdn.alpinelinux.org/alpine/edge/main nginx-mod-http-brotli

# create user
RUN addgroup wwwdata \
  && adduser --uid 1000 --shell /sbin/nologin --system -G 'wwwdata' wwwdata

# fix folder permissions for nginx and remove default config
RUN chown -R wwwdata:wwwdata /var/lib/nginx \
  && chown -R wwwdata:wwwdata /var/log/nginx \
  && rm /etc/nginx/conf.d/default.conf

# add nginx config
ADD ./docker/nginx.conf /etc/nginx
ADD ./docker/ssl.conf /etc/nginx
ADD ./docker/tunnel.conf /etc/nginx/conf.d

# create working folders
RUN mkdir /srv/www \
  && mkdir /srv/dotnet \
  && mkdir /data

# copy blazor files
ADD ./Build/wwwroot /srv/www
COPY ./docker/blazor.appsettings.json /srv/www/appsettings.json

# copy webapi files
ADD ./Build/api /srv/dotnet
COPY ./docker/api.appsettings.json /srv/dotnet/appsettings.json

# fix permissions on working folders
RUN chown -R root:wwwdata /srv/www \
  && chmod g+r /srv/www \
  && chown -R root:wwwdata /srv/dotnet \
  && chmod g+r /srv/dotnet \
  && chown -R wwwdata:wwwdata /data \
  && chown -R root:wwwdata /etc/nginx/conf.d \
  && chmod -R g+w /etc/nginx/conf.d

# change user
USER wwwdata

# add nginx config template
ADD ./docker/ssl.tunnel.conf.tmpl /home/wwwdata

# add volume
VOLUME ["/data"]

EXPOSE 5800

# setup entrypoint
ADD --chown=wwwdata:wwwdata ./docker/docker-entrypoint.sh /
RUN chmod u+x /docker-entrypoint.sh

ENTRYPOINT /docker-entrypoint.sh
