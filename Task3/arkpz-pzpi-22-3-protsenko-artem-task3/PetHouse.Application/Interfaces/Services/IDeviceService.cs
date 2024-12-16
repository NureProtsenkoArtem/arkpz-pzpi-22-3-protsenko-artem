using PetHouse.Core.Enums.Device;
using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface IDeviceService : IGenericService<Device>
{
   Task<Guid> CreateDevice(Guid userId);
   Task<Device> UpdateDevice(Guid deviceId,DeviceStatus deviceStatus,FeedingMode feedingMode,
      bool recognitionEnabled, bool cameraEnabled);
}