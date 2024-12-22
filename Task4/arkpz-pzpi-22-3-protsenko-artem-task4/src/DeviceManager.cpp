#include "DeviceManager.h"

const char *API_DEVICE_PATH = "http://rnanj-91-244-62-30.a.free.pinggy.link/api/Device/";

/**
 * Updates the status of a device, including its feeding mode, device status, 
 * and enabling the recognition and camera features. The information is sent to 
 * the API endpoint via a PUT request.
 */
void DeviceManager::updateDeviceStatus(String deviceId, int deviceStatus, int feedingMode)
{
    HTTPClient http;

    String deviceUrl = String(API_DEVICE_PATH) + deviceId;
    Serial.print("Updating device status via: ");
    Serial.println(deviceUrl);

    http.begin(deviceUrl);
    http.addHeader("Content-Type", "application/json");

    StaticJsonDocument<200> jsonDoc;
    jsonDoc["deviceStatus"] = deviceStatus;
    jsonDoc["feedingMode"] = feedingMode;
    jsonDoc["recognitionEnabled"] = true;
    jsonDoc["cameraEnabled"] = true;

    String requestBody;
    serializeJson(jsonDoc, requestBody);

    Serial.print("Request Body: ");
    Serial.println(requestBody);

    int statusCode = http.PUT(requestBody);

    if (statusCode > 0)
    {
        Serial.print("HTTP Response Code: ");
        Serial.println(statusCode);
    }
    else
    {
        Serial.print("HTTP Request failed, error: ");
        Serial.println(statusCode);
    }

    http.end();
}
