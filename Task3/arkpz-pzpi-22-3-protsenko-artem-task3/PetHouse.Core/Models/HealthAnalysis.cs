using PetHouse.Core.Enums.HealthAnalysis;

namespace PetHouse.Core.Models;

public class HealthAnalysis
{
   public Guid HealthAnalysisId { get; set; }
   public Guid PetId { get; set; }
   public Pet Pet { get; set; }
   public DateTime AnalysisDate { get; set; }
   public DateOnly AnalysisStartDate { get; set; }
   public DateOnly AnalysisEndDate { get; set; }
   public double CaloriesConsumed { get; set; }
   public HealthStatus HealthAnalysisType { get; set; }
   public string Recomendations { get; set; }
}