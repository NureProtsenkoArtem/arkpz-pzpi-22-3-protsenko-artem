using PetHouse.Core.Enums.Meal;

namespace PetHouse.API.Contracts.Meal;

public class ChangeStatusRequest
{
   public MealStatus MealStatus { get; set; }
   public double caloriesConsumed { get; set; }
}