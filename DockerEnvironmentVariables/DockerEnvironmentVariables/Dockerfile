#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DockerEnvironmentVariables/DockerEnvironmentVariables.csproj", "DockerEnvironmentVariables/"]
RUN dotnet restore "DockerEnvironmentVariables/DockerEnvironmentVariables.csproj"
COPY . .
WORKDIR "/src/DockerEnvironmentVariables"
RUN dotnet build "DockerEnvironmentVariables.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerEnvironmentVariables.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerEnvironmentVariables.dll"]
