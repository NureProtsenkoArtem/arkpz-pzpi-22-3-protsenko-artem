using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Persistence.Repositories;

public class MealRepository : GenericRepository<Meal>, IMealRepository
{
   private readonly PetHouseDbContext _context;

   public MealRepository(PetHouseDbContext context) : base(context)
   {
      _context = context;
   }
   public async Task<List<Meal>> FindAll(Expression<Func<Meal, bool>> predicate)
   {
      return await _context.Set<Meal>()
         .Where(predicate)
         .AsNoTracking()
         .ToListAsync();
   }
}