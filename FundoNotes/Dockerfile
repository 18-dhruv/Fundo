# Build context is the SOLUTION folder (see docker-compose.yml),
# because this API depends on BusinessLayer, RepositoryLayer and ModelLayer.

# ---------- Stage 1: build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project files first so "dotnet restore" is cached
# and only re-runs when a .csproj changes, not on every code edit.
COPY ModelLayer/ModelLayer.csproj           ModelLayer/
COPY RepositoryLayer/RepositoryLayer.csproj RepositoryLayer/
COPY BusinessLayer/BusinessLayer.csproj     BusinessLayer/
COPY FundoNotes/FundoNotes.csproj           FundoNotes/
RUN dotnet restore FundoNotes/FundoNotes.csproj

# Now the source code
COPY ModelLayer/      ModelLayer/
COPY RepositoryLayer/ RepositoryLayer/
COPY BusinessLayer/   BusinessLayer/
COPY FundoNotes/      FundoNotes/
RUN dotnet publish FundoNotes/FundoNotes.csproj -c Release -o /app/publish --no-restore

# ---------- Stage 2: runtime (no SDK, much smaller) ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# The aspnet image listens on 8080 and runs as the non-root "app" user by default.
EXPOSE 8080
ENTRYPOINT ["dotnet", "FundoNotes.dll"]
