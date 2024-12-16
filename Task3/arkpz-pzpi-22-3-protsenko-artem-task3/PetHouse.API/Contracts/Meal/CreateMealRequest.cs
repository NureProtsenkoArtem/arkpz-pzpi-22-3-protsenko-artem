using PetHouse.Core.Enums.Meal;

namespace PetHouse.API.Contracts.Meal;

public class CreateMealRequest
{
   public double PortionSize { get; set; }
   public DateTime StartTime { get; set; }
   public double CalorificValue { get; set; }
   public bool AdaptiveAdjustment { get; set; }
   public string FoodType { get; set; }
   public bool IsDaily { get; set; }
}
