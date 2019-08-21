FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ConferencePlanner.sln ./
COPY BackEnd/BackEnd.csproj BackEnd/
COPY FrontEnd/FrontEnd.csproj FrontEnd/
COPY ConferenceDTO/ConferenceDTO.csproj ConferenceDTO/
RUN dotnet restore
COPY . .
WORKDIR /src/BackEnd
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BackEnd.dll"]