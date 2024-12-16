using PetHouse.Core.Enums.Meal;
using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface IMealService : IGenericService<Meal>
{
   Task<Guid> AddMeal(Guid petId, double portionSize, DateTime startTime, bool adaptiveAdjustment,
      string foodType, bool isDaily,double calorificValue);

   Task<Meal> UpdateMeal(Guid mealId, double portionSize, DateTime startTime,
      double caloriesConsumed, MealStatus mealStatus, bool adaptiveAdjustment,
      string foodType, bool isDaily, double calorificValue);
   double CalculateCaloriesPerDay(Pet pet);
   Task<List<Meal>> GetByPetId(Guid petId);
   Task ChangeStatus(Guid meailId, MealStatus mealStatus,double caloriesConsumed);
}