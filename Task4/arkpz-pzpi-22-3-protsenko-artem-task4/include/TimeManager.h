#ifndef TIMEMANAGER_H
#define TIMEMANAGER_H

#include <WiFi.h>
#include "time.h"
class TimeManager
{
public:
  static void setupTime(const char *ntpServer = "pool.ntp.org", long gmtOffsetSec = 0, int daylightOffsetSec = 0);
  static bool isTimeToEat(const String &startTime, bool isDaily, int mealStatus);
  static time_t getCurrentTime();
  static String getFormattedTime();

private:
  static const char *defaultNtpServer;
  static long defaultGmtOffsetSec;
  static int defaultDaylightOffsetSec;
};

#endif
