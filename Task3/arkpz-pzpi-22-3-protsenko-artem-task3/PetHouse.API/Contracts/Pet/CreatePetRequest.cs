using PetHouse.Core.Enums.Pet;

namespace PetHouse.API.Contracts.Pet;

public class CreatePetRequest
{
   public string PetName { get; set; }
   public string PetBreed { get; set; }
   public double PetWeight { get; set; }
   public double CaloriesPerDay { get; set; }
   public ActivityLevel ActivityLevel { get; set; }
}