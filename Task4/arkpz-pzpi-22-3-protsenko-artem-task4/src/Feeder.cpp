#include "Feeder.h"

/**
 * Activates the feeder and dispenses the specified portion of food in grams.
 * The function updates the device status to "Feeding" and "Automatic" and dispenses 
 * the food in increments of 20 grams, simulating the feeding process.
 */
void Feeder::activateFeeder(const String &deviceId, int portionGrams)
{
  Serial.println("Activating feeder...");
  DeviceManager::updateDeviceStatus(deviceId, Feeding, Automatic);
  int gramsDispensed = 0;
  while (gramsDispensed < portionGrams)
  {
    delay(1000);

    gramsDispensed += 20;
    Serial.print("Dispensed: ");
    Serial.println(gramsDispensed);
  }
  Serial.println("Feeding complete!");
}
