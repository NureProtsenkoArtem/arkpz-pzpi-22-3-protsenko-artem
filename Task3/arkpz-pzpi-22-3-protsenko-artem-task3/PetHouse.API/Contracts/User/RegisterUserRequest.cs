using System.ComponentModel.DataAnnotations;

namespace PetHouse.API.Contracts.User;

public record RegisterUserRequest(
   [Required]
   string Name,
   
   [Required]
   [StringLength(20,ErrorMessage ="{0} length must be more than {2} and less than {1} characters", MinimumLength = 8) ]
   string Password,
   
   [Required]
   [EmailAddress]
   string Email
);