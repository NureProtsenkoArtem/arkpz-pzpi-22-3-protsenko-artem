namespace PetHouse.Application.Interfaces.Services;

public interface IPasswordService
{
   string GeneratePassword(int length = 12);
}