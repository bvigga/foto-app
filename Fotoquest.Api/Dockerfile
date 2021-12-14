#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443/tcp

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Fotoquest.Api/Fotoquest.Api.csproj", "Fotoquest.Api/"]
COPY ["Fotoquest.Core/Fotoquest.Core.csproj", "Fotoquest.Core/"]
COPY ["Fotoquest.Data/Fotoquest.Data.csproj", "Fotoquest.Data/"]
RUN dotnet restore "Fotoquest.Api/Fotoquest.Api.csproj"
COPY . .
WORKDIR "/src/Fotoquest.Api"
RUN dotnet build "Fotoquest.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fotoquest.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fotoquest.Api.dll"]
