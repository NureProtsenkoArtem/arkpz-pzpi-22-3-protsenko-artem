using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Contracts.User;
using PetHouse.API.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.User;
using Swashbuckle.AspNetCore.Annotations;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
   private readonly IUserService _userService;

   public UserController(IUserService userService)
   {
      _userService = userService;
   }

   [HttpDelete("{userId:guid}")]
   [SwaggerOperation("Delete user by id")]
   public async Task<ActionResult<Guid>> DeleteUser(Guid userId)
   {
      var userDeletionResult = await _userService.DeleteAsync(userId);

      return Ok(userDeletionResult);
   }

   [HttpPut]
   [SwaggerOperation("Edit user")]
   [Authorize]
   public async Task<IActionResult> EditUser([FromBody] EditUserRequest request)
   {
      var userUpdateResult =
         await _userService.UpdateUser(request.UserId, request.Name, request.Email, request.UserRole);
      return Ok();
   }

   [HttpGet]
   [SwaggerOperation("Get all users")]
   public async Task<IActionResult> GetAll()
   {
      var users = await _userService.GetAll();
      return Ok(users);
   }

   [HttpGet("{userId:Guid}")]
   [SwaggerOperation("Get user by id")]
   [Authorize]
   public async Task<IActionResult> GetUser(Guid userId)
   {
      var user = await _userService.GetById(userId);
      return Ok(user);
   }

   [HttpPost("reset-password")]
   [SwaggerOperation("Generate new user password")]
   public async Task<IActionResult> ResetUserPassword([FromBody] ResetPasswordRequest request)
   {
      await _userService.ResetUserPassword(request.Email);
      return Ok(new { Message = "Password successfully reset" });
   }

   [HttpPost("change-password")]
   [SwaggerOperation("Change user password")]
   public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest request)
   {
      await _userService.ChangeUserPassword(request.UserId, request.OldPassword, request.NewPassword);
      return Ok(new { Message = "Password successfully changed" });
   }
}