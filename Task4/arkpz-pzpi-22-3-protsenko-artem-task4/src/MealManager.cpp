#include "MealManager.h"
#include "TimeManager.h"
#include "Feeder.h"
#include <HTTPClient.h>
#include <vector>

const char *API_MEAL_PATH = "http://rnanj-91-244-62-30.a.free.pinggy.link/api/Meal/";

/**
 * Fetches meal data for the specified user and checks meal schedules for feeding.
 * Activates feeding if the meal time is due and handles adaptive recommendations for the next meal time.
 */
void MealManager::fetchAndCheckMeals(const String &deviceId, const String &userId)
{
  const char *API_PATH = "user/";
  std::vector<std::string> pastMealTimes;
  HTTPClient http;
  String url = String(API_MEAL_PATH) + String(API_PATH) + userId;
  Serial.println("Fetch meal url" + url);

  http.begin(url);
  http.addHeader("Content-Type", "application/json");

  int statusCode = http.GET();
  if (statusCode > 0)
  {
    String response = http.getString();
    StaticJsonDocument<1024> doc;
    DeserializationError error = deserializeJson(doc, response);

    if (!error)
    {
      JsonArray meals = doc.as<JsonArray>();
      for (JsonVariant meal : meals)
      {
        pastMealTimes.push_back(meal["startTime"].as<String>().c_str());
        int mealStatus = meal["mealStatus"].as<int>();

        if (TimeManager::isTimeToEat(meal["startTime"], meal["isDaily"].as<bool>(), mealStatus))
        {
          Feeder::activateFeeder(deviceId, meal["portionSize"].as<int>());
          int portionConsumed = generateRandomMealResult(meal["portionSize"]);
          double consumedCalories = calculateConsumedCalories(portionConsumed, meal["calorificValue"]);
          UpdateMeal(meal["mealId"], 2, consumedCalories);

          if (meal["isDaily"].as<bool>() == true)
          {
            MealManager::CreateMeal(meal);
            String recommendations = MealManager::predictNextMealTime(pastMealTimes, static_cast<int>(pastMealTimes.size()));
            Serial.println("Recommended time: " + recommendations);
          }
        }
      }
    }
  }

  http.end();
}

/**
 * Generates a random meal consumption result based on portion size.
 * Ensures randomness is initialized only once.
 */
int MealManager::generateRandomMealResult(int portionSize)
{
  static bool isSeedInitialized = false;
  if (!isSeedInitialized)
  {
    randomSeed(millis());
    isSeedInitialized = true;
  }

  int result = random(0, portionSize + 1);
  return result;
}

/**
 * Calculates consumed calories based on portion consumed and calorific value.
 */
double MealManager::calculateConsumedCalories(int portionConsumed, int calorificValue)
{
  return (portionConsumed * calorificValue) / 100.0;
}

/**
 * Creates a new meal entry for daily recurring meals with adjusted start time.
 */
void MealManager::CreateMeal(JsonVariant meal)
{
  HTTPClient http;

  String startTime = meal["startTime"].as<String>();

  int year, month, day, hour, minute, second;
  sscanf(startTime.c_str(), "%4d-%2d-%2dT%2d:%2d:%2d", &year, &month, &day, &hour, &minute, &second);

  struct tm timeinfo = {0};
  timeinfo.tm_year = year - 1900;
  timeinfo.tm_mon = month - 1;
  timeinfo.tm_mday = day;
  timeinfo.tm_hour = hour + 2;
  timeinfo.tm_min = minute;
  timeinfo.tm_sec = second;

  time_t rawTime = mktime(&timeinfo);
  rawTime += 24 * 60 * 60; // Add 24 hours

  struct tm *newTime = localtime(&rawTime);

  char newStartTime[20];
  strftime(newStartTime, sizeof(newStartTime), "%Y-%m-%dT%H:%M:%S", newTime);

  const char *API_PATH = "";
  String creationMeal = String(API_MEAL_PATH) + String(API_PATH) + String(meal["petId"].as<String>());
  http.begin(creationMeal);
  http.addHeader("Content-Type", "application/json");

  StaticJsonDocument<200> jsonDoc;
  jsonDoc["portionSize"] = meal["portionSize"].as<int>();
  jsonDoc["startTime"] = newStartTime;
  jsonDoc["foodType"] = meal["foodType"].as<String>();
  jsonDoc["adaptiveAdjustment"] = meal["adaptiveAdjustment"].as<bool>();
  jsonDoc["calorificValue"] = meal["calorificValue"].as<int>();
  jsonDoc["isDaily"] = meal["isDaily"].as<bool>();

  String requestBody;
  serializeJson(jsonDoc, requestBody);
  int statusCode = http.POST(requestBody);

  if (statusCode > 0)
  {
    String response = http.getString();
    Serial.println(response);
  }

  http.end();
}

/**
 * Updates the status and consumed calories of a meal.
 */
void MealManager::UpdateMeal(const String &mealId, int mealStatus, double consumedCalories)
{
  const char *API_PATH = "change-status/";
  HTTPClient http;

  String mealUrl = String(API_MEAL_PATH) + String(API_PATH) + mealId;
  http.begin(mealUrl);
  http.addHeader("Content-Type", "application/json");

  StaticJsonDocument<200> jsonDoc;
  jsonDoc["mealStatus"] = mealStatus;
  jsonDoc["caloriesConsumed"] = consumedCalories;

  String requestBody;
  serializeJson(jsonDoc, requestBody);
  int statusCode = http.POST(requestBody);

  if (statusCode > 0)
  {
    String response = http.getString();
    Serial.println(response);
  }

  http.end();
}

/**
 * Predicts the next meal time using a moving average of past intervals.
 */
String MealManager::predictNextMealTime(const std::vector<std::string> &pastMealTimes, int smoothingFactor)
{
  if (pastMealTimes.size() < 2)
  {
    Serial.println("Not enough data.");
    return "";
  }

  std::vector<int> intervals;
  for (size_t i = 1; i < pastMealTimes.size(); ++i)
  {
    int time1 = parseTimeToSeconds(pastMealTimes[i - 1].c_str());
    int time2 = parseTimeToSeconds(pastMealTimes[i].c_str());
    intervals.push_back(time2 - time1);
  }

  double weightedSum = 0;
  double weightTotal = 0;
  for (size_t i = 0; i < intervals.size(); ++i)
  {
    double weight = 1.0 / (i + 1);
    weightedSum += intervals[i] * weight;
    weightTotal += weight;
  }
  int predictedInterval = static_cast<int>(weightedSum / weightTotal);

  int lastMealTime = parseTimeToSeconds(pastMealTimes.back().c_str());
  int nextMealTime = lastMealTime + predictedInterval;

  return convertSecondsToISO8601(nextMealTime);
}

/**
 * Parses an ISO8601-formatted time string into seconds since the epoch.
 */
int MealManager::parseTimeToSeconds(const char *timeISO8601)
{
  int year, month, day, hour, minute, second;
  sscanf(timeISO8601, "%4d-%2d-%2dT%2d:%2d:%2d", &year, &month, &day, &hour, &minute, &second);
  struct tm timeinfo = {0};
  timeinfo.tm_year = year - 1900;
  timeinfo.tm_mon = month - 1;
  timeinfo.tm_mday = day;
  timeinfo.tm_hour = hour;
  timeinfo.tm_min = minute;
  timeinfo.tm_sec = second;

  return mktime(&timeinfo);
}

/**
 * Converts seconds since the epoch to an ISO8601-formatted time string.
 */
String MealManager::convertSecondsToISO8601(int secondsSinceEpoch)
{
  struct tm *timeinfo = localtime((time_t *)&secondsSinceEpoch);
  char buffer[20];
  strftime(buffer, sizeof(buffer), "%Y-%m-%dT%H:%M:%S", timeinfo);
  return String(buffer);
}
