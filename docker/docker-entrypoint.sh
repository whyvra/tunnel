#!/bin/sh

# check if ssl variables are set and override tunnel config
if [ ! -z "$SSL_CERT" ] && [ ! -z "$SSL_KEY" ]; then
  envsubst < /home/wwwdata/ssl.tunnel.conf.tmpl | sed -e 's/§/$/g' > /etc/nginx/conf.d/tunnel.conf
fi

# start nginx daemon
nginx

# start dotnet
cd /srv/dotnet
dotnet /srv/dotnet/Whyvra.Tunnel.Api.dll --urls "http://localhost:5000"