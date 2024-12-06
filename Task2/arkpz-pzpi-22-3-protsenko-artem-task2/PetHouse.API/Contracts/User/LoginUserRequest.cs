using System.ComponentModel.DataAnnotations;

namespace PetHouse.API.Contracts.User;

public record LoginUserRequest(
   
   [Required] 
   [EmailAddress]
   string Email,
   
   [Required] 
   string Password
);