using System.Linq.Expressions;
using PetHouse.Core.Models;

namespace PetHouse.Persistence.Interfaces;

public interface IMealRepository : IRepository<Meal>
{
   Task<List<Meal>> FindAll(Expression<Func<Meal, bool>> predicate);
}