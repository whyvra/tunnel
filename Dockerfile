FROM whyvra/dotnet-nginx-brotli:5.0

# add nginx config for webapp
ADD ./docker/tunnel.conf /etc/nginx/conf.d

# copy blazor files
ADD ./Build/wwwroot /srv/www

# copy webapi files
ADD ./Build/api /srv/dotnet
COPY ./docker/api.appsettings.json /srv/dotnet/appsettings.json

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
