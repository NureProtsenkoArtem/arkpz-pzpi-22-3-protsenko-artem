using PetHouse.Core.Enums.Pet;

namespace PetHouse.Core.Models;

public class Pet
{
   public Guid PetId { get; set; }
   public Guid UserId { get; set; }
   public User? User { get; set; }
   public string PetName { get; set; }
   public string PetType { get; set; }
   public double PetWeight { get; set; }
   public double CaloriesPerDay { get; set; }
   public ActivityLevel ActivityLevel { get; set; }
   public string RecognizableData { get; set; }
   public List<Meal> Meals { get; set; }
   public List<HealthAnalysis> PetHealthAnalyses { get; set; }
}