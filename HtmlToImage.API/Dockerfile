#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HtmlToImage.API/HtmlToImage.API.csproj", "HtmlToImage.API/"]
RUN dotnet restore "HtmlToImage.API/HtmlToImage.API.csproj"
COPY . .
WORKDIR "/src/HtmlToImage.API"
RUN dotnet build "HtmlToImage.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HtmlToImage.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HtmlToImage.API.dll"]

######################
##PUPPETEER RECIPE
######################
## Install latest chrome dev package and fonts to support major charsets (Chinese, Japanese, Arabic, Hebrew, Thai and a few others)
## Note: this installs the necessary libs to make the bundled version of Chromium that Puppeteer
## installs, work.
RUN apt-get -o Acquire::Check-Valid-Until=false -o Acquire::Check-Date=false update &&      \
    apt-get -o Acquire::Check-Valid-Until=false -o Acquire::Check-Date=false install -y -f  \
    wget        \
    gnupg2      \
    apt-utils

RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
    && sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' \
    && apt-get -o Acquire::Check-Valid-Until=false -o Acquire::Check-Date=false update \
    && apt-get install -y google-chrome-unstable fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst \
      --no-install-recommends \
    && rm -rf /var/lib/apt/lists/*
######################
##END PUPPETEER RECIPE
######################

# It's a good idea to use dumb-init to help prevent zombie chrome processes.
ADD https://github.com/Yelp/dumb-init/releases/download/v1.2.0/dumb-init_1.2.0_amd64 /usr/local/bin/dumb-init
RUN chmod +x /usr/local/bin/dumb-init

ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-unstable"