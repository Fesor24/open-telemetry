FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR src
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG CONFIGURATION=Release
WORKDIR /src
COPY ["Country.Api/Country.Api.csproj", "Country.Api/"]
RUN dotnet restore "Country.Api/Country.Api.csproj"
COPY ["Country.Infrastructure/", "Country.Infrastructure/"]
COPY ["Country.Domain/", "Country.Domain/"]
COPY ["Country.Application/", "Country.Application/"]
COPY ["Country.Api/", "Country.Api/"]
WORKDIR /src/Country.Api
RUN dotnet build -c $CONFIGURATION -o /src/build

FROM build as publish
ARG CONFIGURATION=Release
RUN dotnet publish -c $CONFIGURATION -o /src/publish /p:UseAppHost=false

FROM base as final
WORKDIR /src
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "Country.Api.dll"]