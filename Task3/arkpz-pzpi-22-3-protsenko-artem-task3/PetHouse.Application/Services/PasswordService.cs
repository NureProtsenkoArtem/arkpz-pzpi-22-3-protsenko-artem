using PetHouse.Application.Interfaces.Services;

namespace PetHouse.Application.Services;

public class PasswordService : IPasswordService
{
   private static readonly string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
   
   public string GeneratePassword(int length = 12)
   {
      var random = new Random();
      
      //Generates password from AllowedChars field
      return new string(Enumerable.Range(0, length)
         .Select(_ => AllowedChars[random.Next(AllowedChars.Length)])
         .ToArray());
   }
}