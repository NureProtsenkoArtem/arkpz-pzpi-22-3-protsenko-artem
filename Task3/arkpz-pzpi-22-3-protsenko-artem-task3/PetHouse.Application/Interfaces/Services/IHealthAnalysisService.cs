using PetHouse.Core.Enums.HealthAnalysis;
using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface IHealthAnalysisService : IGenericService<HealthAnalysis>
{
   Task<Guid> CreateHealthAnalysis(Guid petId, DateOnly startAnalysisDate,
      DateOnly endAnalysisDate);

   Task<HealthAnalysis> UpdateHealthAnalysis(Guid healthAnalysisId,DateOnly startAnalysisDate,
      DateOnly endAnalysisDate,HealthStatus healthAnalysisType, string recommendations);
}