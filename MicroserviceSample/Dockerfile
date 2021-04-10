FROM mcr.microsoft.com/dotnet/runtime:5.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["MicroserviceSample/MicroserviceSample.csproj", "MicroserviceSample/"]
RUN dotnet restore "MicroserviceSample/MicroserviceSample.csproj"
COPY . .
WORKDIR "/src/MicroserviceSample"
RUN dotnet build "MicroserviceSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceSample.dll"]