#ifndef CONFIG_PARAMETERS_H
#define CONFIG_PARAMETERS_H

enum DeviceStatus {
   Offline = 1,
   Online,
   Idle,
   Feeding,
   Error,
};

enum FeedingMode
{
    Manual = 1,
    Automatic,
    Adaptive,
    Custom,
    Pause,
};

const char* getDeviceStatus(DeviceStatus param);
const char* getFeedingMode(FeedingMode param);

#endif 