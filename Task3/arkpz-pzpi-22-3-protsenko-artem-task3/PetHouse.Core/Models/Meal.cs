using System.ComponentModel;
using System.Security.Cryptography;
using PetHouse.Core.Enums.Meal;

namespace PetHouse.Core.Models;

public class Meal
{
   public Guid MealId { get; set; }
   public Guid PetId { get; set; }
   public Pet? Pet { get; set; }
   public double  PortionSize { get; set; }
   public DateTime StartTime { get; set; }
   public double  CaloriesPerMeal { get; set; }
   public double CaloriesConsumed { get; set; }
   public bool AdaptiveAdjustment { get; set; }
   public string FoodType { get; set; }
   public double CalorificValue { get; set; }
   public MealStatus MealStatus { get; set; }
   public bool IsDaily { get; set; }
}