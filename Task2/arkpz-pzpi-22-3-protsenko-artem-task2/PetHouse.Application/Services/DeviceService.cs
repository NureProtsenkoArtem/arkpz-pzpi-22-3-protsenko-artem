using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.Device;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;
using PetHouse.Persistence.Repositories;

namespace PetHouse.Application.Services;

public class DeviceService : GenericService<Device>, IDeviceService
{
   private readonly IUnitOfWork _unitOfWork;

   public DeviceService(IUnitOfWork unitOfWork) : base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }

   public async Task<Guid> CreateDevice(Guid userId)
   {
      
      var userRepository = _unitOfWork.Repository<User>();
      var user = await userRepository.FindById(userId);
      if (user == null)
      {
         throw new ApiException("User wasn't found", 404);
      }
      
      var device = new Device
      {
         DeviceId = Guid.NewGuid(),
         UserId = userId,
         Model = "Default Model",
         DeviceStatus = DeviceStatus.Offline,
         FeedingMode = FeedingMode.Pause,
         RecognitionEnabled = false,
         CameraEnabled = false
      };

      await Repository.Add(device);
      
      await _unitOfWork.SaveChangesAsync();

      return device.DeviceId;
   }

   public async Task<Device> UpdateDevice(Guid deviceId, DeviceStatus deviceStatus, FeedingMode feedingMode,
      bool recognitionEnabled,bool cameraEnabled)
   {
      var device = await Repository.FindById(deviceId);

      if (device == null)
         throw new ApiException("Device wasn't found", 404);

      device.DeviceStatus = deviceStatus;
      device.FeedingMode = feedingMode;
      device.RecognitionEnabled = recognitionEnabled;
      device.CameraEnabled = cameraEnabled;

      await Repository.Update(device);
      await _unitOfWork.SaveChangesAsync();

      return device;
   }
}