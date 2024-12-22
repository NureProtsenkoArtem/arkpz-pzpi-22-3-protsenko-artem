#include "WiFiManager.h"

const char *WIFI_SSID = "Wokwi-GUEST";
const char *WIFI_PASSWORD = "";

WebServer server(80);

// Function to connect to WiFi using predefined SSID and password
void WiFiManager::connectToWiFi()
{
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");
}

// Function to handle the root URL and display a form for WiFi setup
void WiFiManager::handleRoot()
{
  String html = "<h1>WiFi Setup</h1>"
                "<form action='/setWiFi' method='POST'>"
                "<label>SSID: </label><input type='text' name='ssid'><br>"
                "<label>Password: </label><input type='password' name='password'><br>"
                "<input type='submit' value='Connect'>"
                "</form>";
  server.send(200, "text/html", html);
}

// Function to handle the WiFi setup form submission
void WiFiManager::handleSetWiFi()
{
  if (server.hasArg("ssid") && server.hasArg("password"))
  {
    String ssid = server.arg("ssid");
    String password = server.arg("password");

    Serial.print("Received SSID: ");
    Serial.println(ssid);
    Serial.print("Received Password: ");
    Serial.println(password);

    WIFI_SSID = ssid.c_str();
    WIFI_PASSWORD = password.c_str();

    server.send(200, "text/plain", "WiFi credentials received. Rebooting...");
    delay(2000);
    ESP.restart();
  }
  else
  {
    server.send(400, "text/plain", "SSID and Password are required!");
  }
}
