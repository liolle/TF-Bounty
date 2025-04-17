FROM mcr.microsoft.com/mssql/server

ARG INVALIDATE_CACHE
USER root
# Create a config directory
RUN mkdir -p /usr/config
WORKDIR /usr/config

# Bundle config source
COPY ./config .

# Grant permissions for to our scripts to be executable
RUN chmod +x ./entrypoint.sh
RUN chmod +x ./init-db.sh

USER mssql

ENTRYPOINT ["./entrypoint.sh"]
