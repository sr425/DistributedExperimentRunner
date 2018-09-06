from microsoft/dotnet:sdk AS build

COPY . /src/controller
WORKDIR /src/controller

RUN apt-get update && apt-get install -y curl apt-transport-https apt-utils

RUN curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | apt-key add -
RUN echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list

RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get update && apt-get -y install yarn

RUN dotnet publish -c Release -o ../../bin/controller

from microsoft/dotnet:aspnetcore-runtime

WORKDIR /controller/
COPY --from=build /bin/controller ./