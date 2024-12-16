using System.ComponentModel.DataAnnotations;

namespace PetHouse.API.Contracts.User;

public class ChangePasswordRequest
{
   [Required]
   public Guid UserId { get; set; }
   
   [Required]
   public string OldPassword { get; set; }
   
   [Required]
   [StringLength(20,ErrorMessage ="{0} length must be more than {2} and less than {1} characters", MinimumLength = 8) ]
   public string NewPassword { get; set; }
}