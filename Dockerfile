#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Geolocator.API/Geolocator.API.csproj", "./Geolocator.API/"]
COPY ["./Geolocator.Services/Geolocator.Services.csproj", "./Geolocator.Services/"]
COPY ["./Geolocator.Client/Geolocator.Client.csproj", "./Geolocator.Client/"]
COPY ["./Geolocator.Data/Geolocator.Data.csproj", "./Geolocator.Data/"]
COPY ["./Geolocator.DTO/Geolocator.DTO.csproj", "./Geolocator.DTO/"]
RUN dotnet restore "./Geolocator.API/Geolocator.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Geolocator.API/Geolocator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Geolocator.API/Geolocator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Geolocator.API.dll"]