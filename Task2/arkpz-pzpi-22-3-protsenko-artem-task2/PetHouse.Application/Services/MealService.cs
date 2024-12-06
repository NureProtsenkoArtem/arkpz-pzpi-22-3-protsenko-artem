using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.Meal;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;

public class MealService : GenericService<Meal>, IMealService
{
   private readonly IUnitOfWork _unitOfWork;

   public MealService(IUnitOfWork unitOfWork) : base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }

   public async Task<Guid> AddMeal(Guid petId, double portionSize, DateTime startTime, double caloriesPerDay,
       bool adaptiveAdjustment,string foodType, bool isDaily)
   {
      var petRepository = _unitOfWork.Repository<Pet>();

      var pet = await petRepository.FindById(petId);

      if (pet == null)
         throw new ApiException("Pet wasn't found", 404);

      var meal = new Meal
      {
         MealId = Guid.NewGuid(),
         PetId = petId,
         AdaptiveAdjustment = adaptiveAdjustment,
         CaloriesPerMeal = caloriesPerDay,
         FoodType = foodType,
         IsDaily = isDaily,
         MealStatus = MealStatus.Scheduled,
         PortionSize = portionSize,
         StartTime = startTime,
      };

      await Repository.Add(meal);
      await _unitOfWork.SaveChangesAsync();

      return meal.MealId;
   }
   

   public async Task<Meal> UpdateMeal(Guid mealId, double portionSize, DateTime startTime, 
      double caloriesPerDay, double caloriesConsumed, MealStatus mealStatus,
      bool adaptiveAdjustment, string foodType, bool isDaily)
   {
      var meal = await Repository.FindById(mealId);

      if (meal == null)
         throw new ApiException("Pet wasn't found", 404);

      meal.PortionSize = portionSize;
      meal.StartTime = startTime;
      meal.CaloriesPerMeal = caloriesPerDay;
      meal.CaloriesConsumed = caloriesConsumed;
      meal.AdaptiveAdjustment = adaptiveAdjustment;
      meal.FoodType = foodType;
      meal.MealStatus = mealStatus;
      meal.IsDaily = isDaily;

      await Repository.Update(meal);
      await _unitOfWork.SaveChangesAsync();

      return meal;
   }
}