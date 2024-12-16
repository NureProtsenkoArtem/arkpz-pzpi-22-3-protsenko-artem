using PetHouse.Core.Enums.HealthAnalysis;

namespace PetHouse.API.Contracts.HealthAnalysis;

public class UpdateHealthAnalysisRequest
{
   public HealthStatus HealthAnalysisType { get; set; }
   public DateOnly StartAnalysisDate { get; set; }
   public DateOnly EndAnalysisDate { get; set; }
   public string Recomendations { get; set; }
}