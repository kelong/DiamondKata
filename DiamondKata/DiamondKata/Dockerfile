FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DiamondKata/DiamondKata.csproj", "DiamondKata/"]
COPY ["DiamondKata.Abstraction/DiamondKata.Abstraction.csproj", "DiamondKata.Abstraction/"]
COPY ["DiamondKata.Application/DiamondKata.Application.csproj", "DiamondKata.Application/"]
RUN dotnet restore "DiamondKata/DiamondKata.csproj"
COPY . .
WORKDIR "/src/DiamondKata"
RUN dotnet build "DiamondKata.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DiamondKata.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiamondKata.dll"]
