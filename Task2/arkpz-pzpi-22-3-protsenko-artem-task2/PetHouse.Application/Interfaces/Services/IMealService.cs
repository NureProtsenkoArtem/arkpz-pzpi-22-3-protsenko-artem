using PetHouse.Core.Enums.Meal;
using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface IMealService : IGenericService<Meal>
{
   Task<Guid> AddMeal(Guid petId, double portionSize, DateTime startTime, double caloriesPerDay,
      bool adaptiveAdjustment, string foodType, bool isDaily);

   Task<Meal> UpdateMeal(Guid mealId, double portionSize, DateTime startTime, double caloriesPerDay,
      double caloriesConsumed, MealStatus mealStatus, bool adaptiveAdjustment, string foodType, bool isDaily);
}