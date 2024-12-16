using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Contracts.User;
using PetHouse.Application.Contracts.User;
using PetHouse.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
   private readonly IAuthService _authService;

   public AuthController(IAuthService authService)
   {
      _authService = authService;
   }

   [HttpPost("register")]
   [SwaggerOperation("Registration user into system")]
   public async Task<IActionResult> Register(RegisterUserRequest request)
   {
      await _authService.Register(request.Name, request.Password, request.Email);
      return Ok(new { Message = "User successfully registered" });
   }

   [HttpPost("login")]
   [SwaggerOperation("Authorize user into system")]
   [ProducesResponseType(200, Type = typeof(LoginUserResponse))]
   public async Task<IActionResult> Login(LoginUserRequest request)
   {
      var loginResult = await _authService.Login(request.Email, request.Password);
      HttpContext.Response.Cookies.Append("tasty-cookies", loginResult.RefreshToken, new CookieOptions()
      {
         HttpOnly = true,
         Expires = DateTime.Now.AddDays(30),
         SameSite = SameSiteMode.Strict
      });

      return Ok(loginResult);
   }

   [HttpPost("logout")]
   [SwaggerOperation("Unauthorize user from system")]
   public async Task<IActionResult> Logout()
   {
      HttpContext.Response.Cookies.Delete("tasty-cookies");
      return Ok(new { Message = "Successfully logout" });
   }

   [HttpPost("verify-email")]
   [SwaggerOperation("Verify user account")]
   public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
   {
      await _authService.VerifyEmail(request.Email, request.ActivationCode);
      return Ok(new { Message = "Email successfully verified" });
   }
   
}