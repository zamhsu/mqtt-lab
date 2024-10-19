using MQTTnet.AspNetCore;
using Lab.MQTT.Broker.Events;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // This will allow MQTT connections based on TCP port 1883.
    options.ListenAnyIP(1833, listenOptions => listenOptions.UseMqtt());
    
    // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:80/mqtt"
    // See code below for URI configuration.
    options.ListenAnyIP(80);
});

// builder.Services.AddMqttServer();
builder.Services.AddHostedMqttServer(optionsBuilder =>
{
    optionsBuilder.WithDefaultEndpoint();
});
builder.Services.AddMqttConnectionHandler();
builder.Services.AddConnections();

builder.Services.AddSingleton<MqttEvents>();

var app = builder.Build();

app.UseRouting();

app.MapMqtt("/mqtt");

app.UseMqttServer(server =>
{
    var requiredService = app.Services.GetRequiredService<MqttEvents>();
    server.ClientConnectedAsync += requiredService.OnClientConnected;
    server.ClientDisconnectedAsync += requiredService.OnClientDisconnected;
    server.ValidatingConnectionAsync += requiredService.ValidateConnection;
});

app.Run();