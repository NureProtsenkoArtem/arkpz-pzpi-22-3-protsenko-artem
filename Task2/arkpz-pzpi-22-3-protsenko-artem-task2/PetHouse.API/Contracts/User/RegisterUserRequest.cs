using System.ComponentModel.DataAnnotations;

namespace PetHouse.API.Contracts.User;

public record RegisterUserRequest(
   [Required]
   string Name,
   
   [Required]
   string Password,
   
   [Required]
   [EmailAddress]
   string Email
);