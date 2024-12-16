using PetHouse.Core.Models;

namespace PetHouse.Persistence.Interfaces;

public interface IUserRepository : IRepository<User>
{
   Task<User?> GetByEmail(string email);
}