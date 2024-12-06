using PetHouse.Core.Enums.HealthAnalysis;
using PetHouse.Core.Models;

namespace PetHouse.API.Contracts.HealthAnalysis;

public class CreateHealthAnalysisRequest
{
   public HealthStatus HealthAnalysisType { get; set; }
   public DateOnly StartAnalysisDate { get; set; }
   public DateOnly EndAnalysisDate { get; set; }
}