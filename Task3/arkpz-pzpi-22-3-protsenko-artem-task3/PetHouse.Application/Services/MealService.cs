using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.Meal;
using PetHouse.Core.Enums.Pet;
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

   // Adds a new meal for a pet.
   // - Calculates calories per meal based on adaptive adjustment or portion size.
   // - Stores meal details in the database.
   // - Returns the ID of the newly added meal.
   public async Task<Guid> AddMeal(Guid petId, double portionSize, DateTime startTime, bool adaptiveAdjustment,
      string foodType, bool isDaily, double calorificValue)
   {
      double caloriesPerMeal = 0.0;

      var petRepository = _unitOfWork.Repository<Pet>();

      var pet = await petRepository.FindById(petId);

      if (pet == null)
         throw new ApiException("Pet wasn't found", 404);

      if (adaptiveAdjustment)
      {
         caloriesPerMeal = CalculateCaloriesPerDay(pet);
         portionSize = Math.Round(caloriesPerMeal / calorificValue * 100);
      }
      else
      {
         caloriesPerMeal = CalculateCaloriesPerMeal(portionSize, calorificValue);
      }

      var meal = new Meal
      {
         MealId = Guid.NewGuid(),
         PetId = petId,
         AdaptiveAdjustment = adaptiveAdjustment,
         CaloriesPerMeal = caloriesPerMeal,
         FoodType = foodType,
         IsDaily = isDaily,
         MealStatus = MealStatus.Scheduled,
         PortionSize = portionSize,
         CalorificValue = calorificValue,
         StartTime = startTime,
      };

      await Repository.Add(meal);
      await _unitOfWork.SaveChangesAsync();

      return meal.MealId;
   }

   // Calculates the number of calories per meal based on portion size and calorific value.
   public double CalculateCaloriesPerMeal(double portionSize, double calorificValue, int divider = 100)
   {
      return (portionSize * calorificValue) / divider;
   }

   // Updates an existing meal.
   // - Updates meal details including portion size, calories consumed, and status.
   // - Recalculates calories per meal if adaptive adjustment is enabled.
   // - Saves the updated meal to the database.
   public async Task<Meal> UpdateMeal(Guid mealId, double portionSize, DateTime startTime,
      double caloriesConsumed, MealStatus mealStatus, bool adaptiveAdjustment,
      string foodType, bool isDaily, double calorificValue)
   {
      double caloriesPerMeal = 0.0;

      var meal = await Repository.FindById(mealId);

      if (meal == null)
         throw new ApiException("Meal wasn't found", 404);

      if (adaptiveAdjustment)
      {
         var petRepository = _unitOfWork.Repository<Pet>();

         var pet = await petRepository.FindById(meal.PetId);

         caloriesPerMeal = CalculateCaloriesPerDay(pet);
         portionSize = Math.Round(caloriesPerMeal / calorificValue * 100);
      }
      else
      {
         caloriesPerMeal = CalculateCaloriesPerMeal(portionSize, calorificValue);
      }

      meal.PortionSize = portionSize;
      meal.StartTime = startTime;
      meal.CaloriesPerMeal = caloriesPerMeal;
      meal.CaloriesConsumed = caloriesConsumed;
      meal.AdaptiveAdjustment = adaptiveAdjustment;
      meal.FoodType = foodType;
      meal.MealStatus = mealStatus;
      meal.CalorificValue = calorificValue;
      meal.IsDaily = isDaily;

      await Repository.Update(meal);
      await _unitOfWork.SaveChangesAsync();

      return meal;
   }

   // Calculates the number of calories required per day for a pet.
   // - Uses the pet's weight and activity level to compute the Resting Energy Requirement (RER).
   public double CalculateCaloriesPerDay(Pet pet)
   {
      if (pet.PetWeight <= 0)
         throw new ApiException("Invalid pet weight", 400);

      // Calculate RER (Resting Energy Requirement)
      double rer = 70 * Math.Pow(pet.PetWeight, 0.75);

      double activityFactor = pet.ActivityLevel switch
      {
         ActivityLevel.Low => 1.2,
         ActivityLevel.Moderate => 1.5,
         ActivityLevel.High => 1.8,
         _ => 1.2
      };

      // Calculate calories per day considering the activity factor
      double caloriesPerMeal = rer * activityFactor;

      return caloriesPerMeal;
   }

   // Retrieves all meals associated with a specific pet ID.
   // - Returns an empty list if no meals are found.
   public async Task<List<Meal>> GetByPetId(Guid petId)
   {
      var meals = await Repository.GetByPredicate(m => m.PetId == petId);

      if (meals.Count <= 0)
      {
         return new List<Meal>();
      }

      return meals;
   }

   // Changes the status of a meal and updates the calories consumed.
   public async Task ChangeStatus(Guid mealId, MealStatus mealStatus, double caloriesConsumed)
   {
      var meal = await Repository.FindById(mealId);

      if (meal == null)
         throw new ApiException("Meal not found", 404);

      meal.MealStatus = mealStatus;
      meal.CaloriesConsumed = caloriesConsumed;

      await Repository.Update(meal);
      await _unitOfWork.SaveChangesAsync();
   }
}
