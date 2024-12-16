using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Contracts.Device;
using PetHouse.API.Contracts.Pet;
using PetHouse.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceController : ControllerBase
{
   private readonly IDeviceService _deviceService;

   public DeviceController(IDeviceService deviceService)
   {
      _deviceService = deviceService;
   }

   [HttpGet]
   [SwaggerOperation("Get all devices")]
   [Authorize]
   public async Task<IActionResult> GetAll()
   {
      var devices = await _deviceService.GetAll();
      return Ok(devices);
   }

   [HttpGet("{deviceId:Guid}")]
   [SwaggerOperation("Get device by id")]
   [Authorize]
   public async Task<IActionResult> GetById(Guid deviceId)
   {
      var device = await _deviceService.GetById(deviceId);

      return Ok(device);
   }

   [HttpDelete("{deviceId:Guid}")]
   [SwaggerOperation("Delete pet by id")]
   [Authorize]
   public async Task<IActionResult> Delete(Guid deviceId)
   {
      var deviceDeletionResult = await _deviceService.DeleteAsync(deviceId);

      return Ok(deviceDeletionResult);
   }

   [HttpPost("{userId:Guid}")]
   [SwaggerOperation("Create device")]
   [Authorize]
   public async Task<IActionResult> CreateDevice([FromRoute] Guid userId)
   {
      var deviceId = await _deviceService.CreateDevice(userId);
      return Ok(deviceId);
   }

   [HttpPatch("{deviceId:Guid}")]
   [SwaggerOperation("Update device model")]
   [Authorize]
   public async Task<IActionResult> UpdateDevice([FromRoute] Guid deviceId, [FromBody] UpdateDeviceRequest request)
   {
      var deviceUpdateResult = await _deviceService.UpdateDevice(deviceId, request.DeviceStatus, request.FeedingMode,
         request.RecognitionEnabled, request.CameraEnabled);
      
      return Ok(deviceUpdateResult);
   }
}