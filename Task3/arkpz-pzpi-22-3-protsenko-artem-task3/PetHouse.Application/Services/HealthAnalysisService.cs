using PetHouse.Application.Contracts.HealthAnalysis;
using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.HealthAnalysis;
using PetHouse.Core.Enums.Meal;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;

public class HealthAnalysisService : GenericService<HealthAnalysis>, IHealthAnalysisService
{
   private readonly IUnitOfWork _unitOfWork;
   private readonly IMealRepository _mealRepository;
   private readonly IMealService _mealService;


   public HealthAnalysisService(IUnitOfWork unitOfWork,
      IMealRepository mealRepository, IMealService mealService) : base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
      _mealRepository = mealRepository;
      _mealService = mealService;
   }

   // Creates a new health analysis for a pet within a specified date range.
   // - Fetches the pet's data and meal history for the given period.
   // - Performs analysis based on the meal data to determine the health status and recommendations.
   public async Task<Guid> CreateHealthAnalysis(Guid petId, DateOnly startDate, DateOnly endDate)
   {
      var petRepository = _unitOfWork.Repository<Pet>();

      var pet = await petRepository.FindById(petId);

      if (pet == null)
      {
         throw new ApiException("User wasn't found", 404);
      }

      var startDateTime = DateTime.SpecifyKind(startDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
      var endDateTime = DateTime.SpecifyKind(endDate.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);

      var meals = await _mealRepository
         .FindAll(m => m.PetId == petId &&
                       m.StartTime >= startDateTime &&
                       m.StartTime <= endDateTime &&
                       m.MealStatus == MealStatus.Completed);

      if (!meals.Any())
      {
         throw new ApiException("No meals found for the specified period", 400);
      }

      var analysisResult = AnalyzeHealth(pet, meals);

      var healthAnalysis = new HealthAnalysis
      {
         HealthAnalysisId = Guid.NewGuid(),
         PetId = petId,
         AnalysisDate = DateTime.UtcNow,
         AnalysisStartDate = startDate,
         AnalysisEndDate = endDate,
         CaloriesConsumed = analysisResult.TotalCalories,
         HealthAnalysisType = analysisResult.HealthStatus,
         Recomendations = analysisResult.Recommendations
      };

      await Repository.Add(healthAnalysis);
      await _unitOfWork.SaveChangesAsync();

      return healthAnalysis.HealthAnalysisId;
   }

   // Updates an existing health analysis.
   // - Recalculates the calories consumed during the new date range.
   // - Updates health analysis details like health status and recommendations.
   public async Task<HealthAnalysis> UpdateHealthAnalysis(Guid healthAnalysisId,
      DateOnly startDate, DateOnly endDate, HealthStatus healthAnalysisType, string recommendations)
   {
      var healthAnalysis = await Repository.FindById(healthAnalysisId);

      if (healthAnalysis == null)
         throw new ApiException("Health analysis wasn't found", 404);

      var startDateTime = DateTime.SpecifyKind(startDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
      var endDateTime = DateTime.SpecifyKind(endDate.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);

      var meals = await _mealRepository.FindAll(m => m.PetId == healthAnalysis.PetId &&
                                                     m.StartTime >= startDateTime &&
                                                     m.StartTime <= endDateTime);

      var totalCalories = meals.Sum(m => m.CaloriesConsumed);

      healthAnalysis.CaloriesConsumed = totalCalories;
      healthAnalysis.HealthAnalysisType = healthAnalysisType;
      healthAnalysis.Recomendations = recommendations;

      await Repository.Update(healthAnalysis);
      await _unitOfWork.SaveChangesAsync();

      return healthAnalysis;
   }

   // Analyzes the health of a pet based on its meals.
   // - Calculates total and average calories consumed.
   // - Evaluates consistency of calorie intake using standard deviation and coefficient of variation.
   // - Determines health status and provides dietary recommendations.
   private HealthAnalysisResult AnalyzeHealth(Pet pet, IEnumerable<Meal> meals)
   {
      var totalCalories = CalculateTotalCalories(meals);
      var averageCalories = CalculateAverageCalories(meals);
      var calorieStandardDeviation = CalculateCalorieStandardDeviation(meals, averageCalories);
      var coefficientOfVariation = CalculateCoefficientOfVariation(calorieStandardDeviation, averageCalories);
      var idealCalories = _mealService.CalculateCaloriesPerDay(pet);
      var (healthStatus, recommendations) =
         DetermineHealthStatus(averageCalories, idealCalories, coefficientOfVariation);

      return new HealthAnalysisResult
      {
         TotalCalories = totalCalories,
         HealthStatus = healthStatus,
         Recommendations = recommendations
      };
   }

   // Calculates the total calories consumed from a list of meals.
   private double CalculateTotalCalories(IEnumerable<Meal> meals)
   {
      return meals.Sum(m => m.CaloriesConsumed);
   }

   // Calculates the average calories consumed per meal.
   private double CalculateAverageCalories(IEnumerable<Meal> meals)
   {
      return meals.Average(m => m.CaloriesConsumed);
   }

   // Calculates the standard deviation of calorie intake.
   private double CalculateCalorieStandardDeviation(IEnumerable<Meal> meals, double averageCalories)
   {
      return Math.Sqrt(meals.Average(m => Math.Pow(m.CaloriesConsumed - averageCalories, 2)));
   }

   // Calculates the coefficient of variation (a measure of inconsistency in calorie intake).
   private double CalculateCoefficientOfVariation(double calorieStandardDeviation, double averageCalories)
   {
      return (calorieStandardDeviation / averageCalories) * 100;
   }

   // Determines the health status and provides dietary recommendations based on calorie intake and its consistency.
   private (HealthStatus, string) DetermineHealthStatus(double averageCalories, double idealCalories,
      double coefficientOfVariation)
   {
      if (coefficientOfVariation > 20)
      {
         return (HealthStatus.Inconsistient,
            "The diet appears inconsistent. Regularize feeding times and portion sizes.");
      }
      else if (averageCalories < idealCalories * 0.8)
      {
         return (HealthStatus.UnderWeight, "Increase portion sizes or meal frequency to meet dietary needs.");
      }
      else if (averageCalories > idealCalories * 1.2)
      {
         return (HealthStatus.Overweight, "Reduce portion sizes or limit calorie-dense foods.");
      }
      else
      {
         return (HealthStatus.Healthy, "The pet's diet is consistent and meets nutritional requirements.");
      }
   }
}