#build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./g2hotel-server.csproj" --disable-parallel
RUN dotnet publish "./g2hotel-server.csproj" -c release -o /app --no-restore
#server stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "g2hotel-server.dll"]