FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Sender.API/Sender.API.csproj", "Sender.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Sender.API/Sender.API.csproj"
COPY . .
WORKDIR "/src/Sender.API"
RUN dotnet build "Sender.API.csproj" -c Release -o /app/build
RUN dotnet publish "Sender.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Sender.API.dll"]
