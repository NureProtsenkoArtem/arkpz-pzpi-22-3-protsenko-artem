using PetHouse.Core.Models;

namespace PetHouse.Persistence.Repositories;

public class DeviceRepository : GenericRepository<Device>
{
   public DeviceRepository(PetHouseDbContext context) : base(context)
   {
   }
   
}