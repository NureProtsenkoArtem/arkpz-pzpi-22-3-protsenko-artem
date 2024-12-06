using System.ComponentModel.DataAnnotations;
using PetHouse.Core.Enums.User;

namespace PetHouse.API.Contracts.User;

public class EditUserRequest
{
   [Required]
   public Guid UserId { get; set; }
   
   [Required]
   public string Name { get; set; }
   
   [Required]
   [EmailAddress]
   public string Email { get; set; }

   public Role UserRole { get; set; }
}