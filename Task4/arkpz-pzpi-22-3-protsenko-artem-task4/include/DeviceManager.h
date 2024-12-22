#ifndef DEVICEMANAGER_H
#define DEVICEMANAGER_H

#include <HTTPClient.h>
#include <ArduinoJson.h>

class DeviceManager
{
public:
    static void updateDeviceStatus(String deviceId,int deviceStatus,int feedingMode);
};

#endif