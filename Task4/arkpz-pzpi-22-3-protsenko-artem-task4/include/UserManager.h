#ifndef USERMANAGER_H
#define USERMANAGER_H

#include <ArduinoJson.h>
#include <WiFi.h>

class UserManager {
public:
  static String getUserId(const String& deviceId);
};

#endif