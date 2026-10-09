# Build context is the SOLUTION folder (see docker-compose.yml).
# MessageService has no project references, so only its own folder is needed.

# ---------- Stage 1: build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MessageService/MessageService.csproj MessageService/
RUN dotnet restore MessageService/MessageService.csproj

COPY MessageService/ MessageService/
RUN dotnet publish MessageService/MessageService.csproj -c Release -o /app/publish --no-restore

# ---------- Stage 2: runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MessageService.dll"]
