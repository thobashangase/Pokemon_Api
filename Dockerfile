#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pokemon_Api/Pokemon_Api.csproj", "Pokemon_Api/"]
COPY ["Pokemon_Api.Tests/Pokemon_Api.Tests.csproj", "Pokemon_Api.Tests/"]
RUN dotnet restore "Pokemon_Api/Pokemon_Api.csproj"
COPY . .
WORKDIR "/src/Pokemon_Api"
RUN dotnet build "Pokemon_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokemon_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokemon_Api.dll"]