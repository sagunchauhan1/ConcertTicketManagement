FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VenueService/VenueService.csproj", "VenueService/"]
RUN dotnet restore "VenueService/VenueService.csproj"
COPY . .
WORKDIR "/src/VenueService"
RUN dotnet build "VenueService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VenueService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VenueService.dll"]
