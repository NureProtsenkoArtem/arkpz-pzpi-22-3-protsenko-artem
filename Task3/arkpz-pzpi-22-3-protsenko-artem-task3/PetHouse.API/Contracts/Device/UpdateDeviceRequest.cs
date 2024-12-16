using PetHouse.Core.Enums.Device;

namespace PetHouse.API.Contracts.Device;

public class UpdateDeviceRequest
{
   public DeviceStatus DeviceStatus { get; set; }
   public FeedingMode FeedingMode { get; set; }
   public bool RecognitionEnabled { get; set; }
   public bool CameraEnabled { get; set; }
}