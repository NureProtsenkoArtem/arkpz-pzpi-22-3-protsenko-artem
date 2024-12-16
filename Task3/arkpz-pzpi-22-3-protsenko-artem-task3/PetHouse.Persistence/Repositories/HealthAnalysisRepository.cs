using PetHouse.Core.Models;

namespace PetHouse.Persistence.Repositories;

public class HealthAnalysisRepository : GenericRepository<HealthAnalysis>
{
   public HealthAnalysisRepository(PetHouseDbContext context) : base(context)
   {
   }
   
}