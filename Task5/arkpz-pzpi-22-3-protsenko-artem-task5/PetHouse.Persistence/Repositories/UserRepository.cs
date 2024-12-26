using Microsoft.EntityFrameworkCore;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Persistence.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
   private readonly PetHouseDbContext _context;

   public UserRepository(PetHouseDbContext context) : base(context)
   {
      _context = context;
   }
   
   public async Task<User?> GetByEmail(string email)
   {
      var users = await _context.Users
         .ToListAsync();

      var candidate = users.FirstOrDefault(u => u.Email == email );

      return candidate;
   }
   
}