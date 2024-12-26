using System.Linq.Expressions;

namespace PetHouse.Persistence.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
   Task<List<TEntity>> GetAll();
   Task<Guid> Add(TEntity entity);
   Task<TEntity?> FindById(Guid id);
   Task Update(TEntity item);
   Task<Guid> DeleteAsync(Guid id);
   Task<List<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
}