using PetHouse.Core.Enums.Meal;

namespace PetHouse.API.Contracts.Meal;

public class UpdateMealRequest
{
   public double PortionSize { get; set; }
   public DateTime StartTime { get; set; }
   public double  CaloriesPerMeal { get; set; }
   public MealStatus MealStatus { get; set; }
   public double CaloriesConsumed { get; set; }
   public bool AdaptiveAdjustment { get; set; }
   public string FoodType { get; set; }
   public bool IsDaily { get; set; }
}