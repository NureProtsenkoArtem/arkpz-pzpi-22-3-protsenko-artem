#ifndef FEEDER_H
#define FEEDER_H

#include <Arduino.h>
#include "DeviceManager.h"
#include "DeviceConfig.h"

class Feeder {
public:
  static void activateFeeder(const String& deviceId, int portionGrams);
};

#endif
