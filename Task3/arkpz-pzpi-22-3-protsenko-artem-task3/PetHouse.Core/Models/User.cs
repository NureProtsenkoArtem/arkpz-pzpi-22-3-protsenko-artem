using System.Security.Principal;
using PetHouse.Core.Enums;
using PetHouse.Core.Enums.User;

namespace PetHouse.Core.Models;

public class User
{
   public Guid UserId { get; set; }
   public string Name { get; set; }
   public string Email { get; set; }
   public string Password { get; set; }
   public Role UserRole { get; set; } = Role.User;
   public string? VerificationCode { get; set; }
   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public bool IsVerified { get; set; } = false;
   public List<Pet> UserPets { get; set; } = [];
   public List<Device> UserDevices { get; set; } = [];
}