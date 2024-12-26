using PetHouse.Core.Models;

namespace PetHouse.Persistence.Repositories;

public class PetRepository : GenericRepository<Pet>
{
   public PetRepository(PetHouseDbContext context) : base(context)
   {
   }
   
}