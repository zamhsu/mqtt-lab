FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lab.MQTT.Broker/Lab.MQTT.Broker.csproj", "Lab.MQTT.Broker/"]
RUN dotnet restore "Lab.MQTT.Broker/Lab.MQTT.Broker.csproj"
COPY . .
WORKDIR "/src/Lab.MQTT.Broker"
RUN dotnet build "Lab.MQTT.Broker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Lab.MQTT.Broker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lab.MQTT.Broker.dll"]
