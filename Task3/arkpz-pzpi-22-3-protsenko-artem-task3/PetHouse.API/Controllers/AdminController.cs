using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Helpers;
using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.User;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[RoleVerification(Role.Admin)]
public class AdminController : ControllerBase
{
   private readonly IAdminService _adminService;

   public AdminController(IAdminService adminService)
   {
      _adminService = adminService;
   }

   [HttpGet("backup")]
   public async Task<IActionResult> BackupDatabase([FromQuery] string outputDirectory = null)
   {
      string backupFilePath = await _adminService.BackupData(outputDirectory);
      return Ok(new { message = $"Backup successfully created at {backupFilePath}" });
   }

   [HttpPost("restore")]
   public async Task<IActionResult> RestoreDatabase([FromBody] string backupFilePath)
   {
      await _adminService.RestoreDataAsync(backupFilePath);
      return Ok(new { message = "Database restore completed successfully." });
   }
   
}