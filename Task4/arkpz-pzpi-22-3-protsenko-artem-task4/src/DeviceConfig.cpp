#include "DeviceConfig.h"

const char *getDeviceStatus(DeviceStatus param)
{
    switch (param)
    {
    case Offline:
        return "Offline";
    case Online:
        return "Online";
    case Idle:
        return "Idle";
    case Feeding:
        return "Feeding";
    case Error:
        return "Error";
    default:
        return "";
    }
}

const char *getFeedingMode(FeedingMode param)
{
    switch (param)
    {
    case Manual:
        return "Manual";
    case Automatic:
        return "Automatic";
    case Adaptive:
        return "Adaptive";
    case Custom:
        return "Custom";
    case Pause:
        return "Pause";
    default:
        return "";
    }
}