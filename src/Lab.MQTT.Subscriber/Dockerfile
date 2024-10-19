FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lab.MQTT.Subscriber/Lab.MQTT.Subscriber.csproj", "Lab.MQTT.Subscriber/"]
RUN dotnet restore "Lab.MQTT.Subscriber/Lab.MQTT.Subscriber.csproj"
COPY . .
WORKDIR "/src/Lab.MQTT.Subscriber"
RUN dotnet build "Lab.MQTT.Subscriber.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Lab.MQTT.Subscriber.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lab.MQTT.Subscriber.dll"]
