using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Persistence.Interfaces;
using PetHouse.Persistence.Repositories;

namespace PetHouse.Application.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
{
   private readonly IUnitOfWork _unitOfWork;
   protected IRepository<TEntity> Repository => _unitOfWork.Repository<TEntity>();

   public GenericService(IUnitOfWork unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }
   
   public async Task<Guid> DeleteAsync(Guid entityId)
   {
      var entity = await Repository.FindById(entityId);

      if (entity == null)
      {
         throw new ApiException("Entity wasn't found", 404);
      }

      await Repository.DeleteAsync(entityId);

      await _unitOfWork.SaveChangesAsync();

      return entityId;

   }

   public async Task<TEntity> GetById(Guid entityId)
   {
      var entity = await Repository.FindById(entityId);

      if (entity == null)
      {
         throw new ApiException("Entity wasn't found", 404);
      }

      return entity;
   }

   public Task<List<TEntity>> GetAll()
   {
      return Repository.GetAll();
   }
}