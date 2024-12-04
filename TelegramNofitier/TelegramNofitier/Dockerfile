# Brug et ASP.NET runtime-billede som base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Brug .NET SDK til at bygge applikationen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiér projektfilerne ind i containeren
COPY . .

# Gendan afhængigheder
RUN dotnet restore

# Byg applikationen
RUN dotnet build -c Release -o /app/build

# Udgiv applikationen
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Brug runtime-billedet til at køre applikationen
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Definer standardkommandoen for containeren
ENTRYPOINT ["dotnet", "TelegramNofitier.dll"]
