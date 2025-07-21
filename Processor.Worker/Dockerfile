FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Processor.Worker/Processor.Worker.csproj", "Processor.Worker/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Processor.Worker/Processor.Worker.csproj"
COPY . .
WORKDIR "/src/Processor.Worker"
RUN dotnet build "Processor.Worker.csproj" -c Release -o /app/build
RUN dotnet publish "Processor.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Processor.Worker.dll"]
