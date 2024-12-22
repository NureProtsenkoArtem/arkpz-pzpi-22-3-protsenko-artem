#include "UserManager.h"
#include <HTTPClient.h>

const char* API_SERVER_BASE_URL = "http://rnanj-91-244-62-30.a.free.pinggy.link";

/**
 * Fetches the user ID associated with the given device ID by making a GET request to the API.
 * 
 * @param deviceId The unique identifier of the device.
 * @return The user ID as a string if found, or an empty string otherwise.
 */
String UserManager::getUserId(const String& deviceId) {
  String API_PATH = "/api/Device/" + deviceId;
  String userId;

  HTTPClient http;
  String url = String(API_SERVER_BASE_URL) + API_PATH;
  http.begin(url);
  http.addHeader("Content-Type", "application/json");

  int statusCode = http.GET();
  if (statusCode > 0) {
    String response = http.getString();
    StaticJsonDocument<200> doc;
    DeserializationError error = deserializeJson(doc, response);

    if (!error && doc.containsKey("userId")) {
      userId = doc["userId"].as<String>();
    }
  }

  http.end();

  return userId;
}
