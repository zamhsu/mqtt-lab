# MQTT Lab
MQTT Asp.Net Core 試驗場

使用 `MQTTnet`, `MQTTnet.AspNetCore` 套件來架設 Broker 的簡單範例。

MQTTnet 的 GitHub: [https://github.com/dotnet/MQTTnet](https://github.com/dotnet/MQTTnet)

## 版本
- MQTTnet: 4.3.7.1207
- MQTTnet.AspNetCore: 4.3.7.1207

## 架構
以下服務都使用 ASP.NET Core。

### Lab.MQTT.Broker
開通 1822 port 來走 TCP，80 port 走 WebSocket。

### Lab.MQTT.Publisher
使用 Minimal API 做的簡易發送訊息入口。

呼叫 `PUT /publish`，body 帶入以下格式的資料即可發送。

```json
{
  "Data": "string",
  "TopicId": "string"
}
```

### Lab.MQTT.Subscriber
使用 BackgroundService 做的簡易訂閱服務，接收到訊息就會透過 ILogger 輸出到主控台。