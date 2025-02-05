# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies.
COPY Pets.csproj ./
RUN dotnet restore Pets.csproj

# Copy the remaining source code.
COPY . ./
RUN dotnet build Pets.csproj -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish Pets.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pets.dll"]
