#ifndef WIFIMANAGER_H
#define WIFIMANAGER_H

#include <WiFi.h>
#include <WebServer.h>

class WiFiManager
{
public:
  static void connectToWiFi();
  static void handleRoot();
  static void handleSetWiFi();

};

#endif
