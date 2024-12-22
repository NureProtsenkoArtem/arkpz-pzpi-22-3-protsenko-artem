#ifndef MEALMANAGER_H
#define MEALMANAGER_H

#include <ArduinoJson.h>
#include <WiFi.h>
#include <TimeLib.h>
#include <string>
#include <vector>

class MealManager
{
public:
  static void fetchAndCheckMeals(const String &deviceId, const String &userId);
  static void CreateMeal(JsonVariant meal);
  static void UpdateMeal(const String &mealId, int mealStatus, double consumedCalories);
  static String predictNextMealTime(const std::vector<std::string> &pastMealTimes, int smoothingFactor);
  static int parseTimeToSeconds(const char *timeISO8601);
    static String convertSecondsToISO8601(int secondsSinceEpoch);

private:
  static int generateRandomMealResult(int portionSize);
  static double calculateConsumedCalories(int portionConsumed, int calorificValue);
};

#endif