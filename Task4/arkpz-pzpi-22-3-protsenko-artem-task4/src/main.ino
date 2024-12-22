#include "WifiManager.h"
#include "UserManager.h"
#include "MealManager.h"
#include "TimeManager.h"
#include "DeviceManager.h"
#include "Feeder.h"
#include "DeviceConfig.h"

const char *DEVICE_ID = "ed598e36-33bc-4648-b0e9-564f300cba29";

void setup()
{
  Serial.begin(115200);
  WiFiManager::connectToWiFi();
  DeviceManager::updateDeviceStatus(DEVICE_ID, Online, Pause);
  TimeManager::setupTime();
}

void loop()
{
  String userId = UserManager::getUserId(DEVICE_ID);

  if (!userId.isEmpty())
  {
    Serial.println("Fetching meals...");
    MealManager::fetchAndCheckMeals(DEVICE_ID, userId);
    delay(2000);
  }
  else
  {
    DeviceManager::updateDeviceStatus(DEVICE_ID, Error, Pause);
    Serial.println("Waiting for userId...");
    delay(3000);
  }
}
