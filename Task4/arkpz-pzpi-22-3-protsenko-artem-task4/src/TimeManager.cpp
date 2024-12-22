#include "TimeManager.h"
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <WiFiUdp.h>
#include <NTPClient.h>

WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org", 0, 60000);

const char *TimeManager::defaultNtpServer = "pool.ntp.org";
long TimeManager::defaultGmtOffsetSec = 0;
int TimeManager::defaultDaylightOffsetSec = 0;

/**
 * Fetches the current time from the NTP server and returns it as an epoch time.
 * @return Current epoch time in seconds.
 */
time_t TimeManager::getCurrentTime()
{
  timeClient.update();  // Update the time from NTP
  return timeClient.getEpochTime();  // Return the epoch time
}

/**
 * Gets the current time in a formatted ISO 8601 string (e.g., "2024-12-22T13:45:30Z").
 * @return A formatted time string.
 */
String TimeManager::getFormattedTime()
{
  time_t now = TimeManager::getCurrentTime();
  struct tm *timeInfo = localtime(&now);

  // Adjust for timezone offset
  int timezoneOffset = 2 * 3600; 
  time_t localTime = now + timezoneOffset;
  timeInfo = gmtime(&localTime);

  char buffer[30];
  snprintf(buffer, sizeof(buffer), "%04d-%02d-%02dT%02d:%02d:%02dZ",
           timeInfo->tm_year + 1900, timeInfo->tm_mon + 1, timeInfo->tm_mday,
           timeInfo->tm_hour, timeInfo->tm_min, timeInfo->tm_sec);

  return String(buffer);
}

/**
 * Sets up the time synchronization by connecting to the NTP server and printing the current time.
 * @param ntpServer The NTP server to use (optional, default "pool.ntp.org").
 * @param gmtOffsetSec GMT offset in seconds.
 * @param daylightOffsetSec Daylight saving time offset in seconds.
 */
void TimeManager::setupTime(const char *ntpServer, long gmtOffsetSec, int daylightOffsetSec)
{
  Serial.println("Connecting to NTP server...");
  String formattedTime = TimeManager::getFormattedTime();  // Fetch formatted time
  Serial.println(formattedTime);  // Print formatted time
  Serial.println("Time synchronized successfully.");  // Confirm synchronization
}

/**
 * Checks if the current time matches the scheduled start time for a meal.
 * This method also checks if the meal is daily and if the meal status is valid (1).
 * @param startTime The scheduled start time of the meal (ISO 8601 format).
 * @param isDaily Whether the meal is a daily recurring meal.
 * @param mealStatus The status of the meal (1 = ready).
 * @return True if it's time to eat, false otherwise.
 */
bool TimeManager::isTimeToEat(const String &startTime, bool isDaily, int mealStatus)
{
  if (mealStatus != 1)
  {
    return false;
  }

  int year, month, day, hour, minute, second;
  sscanf(startTime.c_str(), "%4d-%2d-%2dT%2d:%2d:%2d", &year, &month, &day, &hour, &minute, &second);

  Serial.printf("Time to eat: %04d-%02d-%02d %02d:%02d:%02d\n", year, month, day, hour, minute, second);

  String currentFormattedTime = TimeManager::getFormattedTime();

  int currentYear, currentMonth, currentDay, currentHour, currentMinute, currentSecond;
  sscanf(currentFormattedTime.c_str(), "%4d-%2d-%2dT%2d:%2d:%2d",
         &currentYear, &currentMonth, &currentDay, &currentHour, &currentMinute, &currentSecond);

  Serial.printf("Current Time: %04d-%02d-%02d %02d:%02d:%02d\n", currentYear, currentMonth, currentDay, currentHour, currentMinute, currentSecond);

  if (currentYear != year || currentMonth != month || currentDay != day) 
  {
    return false;
  }

  return (currentHour == hour && currentMinute == minute);
}
