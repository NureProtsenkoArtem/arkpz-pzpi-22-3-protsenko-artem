using PetHouse.Application.Services;
using PetHouse.Core.Enums.User;
using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface IUserService : IGenericService<User>
{
   Task<User> UpdateUser(Guid userId, string name, string email, Role userRole);
   Task ResetUserPassword(string email);
   Task ChangeUserPassword(Guid userId, string oldPassword, string newPassword);
}