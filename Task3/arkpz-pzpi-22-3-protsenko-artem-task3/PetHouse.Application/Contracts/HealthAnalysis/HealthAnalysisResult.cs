using PetHouse.Core.Enums.HealthAnalysis;

namespace PetHouse.Application.Contracts.HealthAnalysis;

public class HealthAnalysisResult
{
   public double TotalCalories { get; set; }
   public HealthStatus HealthStatus { get; set; }
   public string Recommendations { get; set; }
}