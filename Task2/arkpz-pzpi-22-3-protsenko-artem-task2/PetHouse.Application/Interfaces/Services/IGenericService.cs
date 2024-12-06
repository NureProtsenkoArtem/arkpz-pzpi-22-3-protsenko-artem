namespace PetHouse.Application.Interfaces.Services;

public interface IGenericService<TEntity> where TEntity : class
{
   Task<Guid> DeleteAsync(Guid entityId);
   Task<TEntity> GetById(Guid entityId);
   Task<List<TEntity>> GetAll();
}