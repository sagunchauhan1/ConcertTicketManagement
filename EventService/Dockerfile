FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventService/EventService.csproj", "EventService/"]
RUN dotnet restore "EventService/EventService.csproj"
COPY . .
WORKDIR "/src/EventService"
RUN dotnet build "EventService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventService.dll"]
