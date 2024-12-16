using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Persistence.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
   private readonly PetHouseDbContext _context;
   private readonly DbSet<T> _dbSet;

   public GenericRepository(PetHouseDbContext context)
   {
      _context = context;
      _dbSet = _context.Set<T>();
   }
   
   public async Task<List<T>> GetAll()
   {
      return await _dbSet.ToListAsync();
   }
   
   public async Task<Guid> Add(T entity)
   {
      if (entity == null)
      {
         throw new ArgumentNullException(nameof(entity));
      }

      await _dbSet.AddAsync(entity);
      await _context.SaveChangesAsync();
      
      var property = entity.GetType().GetProperty("Id") ?? entity.GetType().GetProperty("EntityId");
      if (property != null && property.PropertyType == typeof(Guid))
      {
         return (Guid)property.GetValue(entity);
      }

      return Guid.Empty;
   }
   
   public async Task<T?> FindById(Guid id)
   {
      return await _dbSet.FindAsync(id);
   }
   
   public async Task Update(T entity)
   {
      if (entity == null)
      {
         throw new ArgumentNullException(nameof(entity));
      }

      _dbSet.Update(entity);
      await _context.SaveChangesAsync();
   }
   
   public async Task<Guid> DeleteAsync(Guid id)
   {
      var entity = await FindById(id);
      if (entity == null)
      {
         throw new KeyNotFoundException($"Entity with ID {id} not found.");
      }

      _dbSet.Remove(entity);
      await _context.SaveChangesAsync();
      return id;
   }
   public async Task<List<T>> GetByPredicate(Expression<Func<T, bool>> predicate)
   {
      if (predicate == null)
      {
         throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");
      }

      return await _dbSet.Where(predicate).ToListAsync();
   }
   
}
