using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.HealthAnalysis;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;

public class HealthAnalysisService : GenericService<HealthAnalysis>,IHealthAnalysisService
{
   private readonly IUnitOfWork _unitOfWork;
   private readonly IMealRepository _mealRepository;

   public HealthAnalysisService(IUnitOfWork unitOfWork,IMealRepository mealRepository) : base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
      _mealRepository = mealRepository;
   }

   public async Task<Guid> CreateHealthAnalysis(Guid petId, DateOnly startDate, DateOnly endDate,
      HealthStatus healthAnalysisType)
   {
      var petRepository = _unitOfWork.Repository<Pet>();

      var pet = await petRepository.FindById(petId);
      
      if (pet == null)
      {
         throw new ApiException("User wasn't found", 404);
      }
      
      var startDateTime = DateTime.SpecifyKind(startDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
      var endDateTime = DateTime.SpecifyKind(endDate.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);
      
      var meals = await _mealRepository.FindAll(m => m.PetId == petId && 
                                               m.StartTime >= startDateTime &&
                                               m.StartTime <= endDateTime);
      
      var totalCalories = meals.Sum(m => m.CaloriesConsumed);

      var healthAnalysis = new HealthAnalysis
      {
         HealthAnalysisId = Guid.NewGuid(),
         PetId = petId,
         AnalysisDate = DateTime.UtcNow,
         CaloriesConsumed = totalCalories,
         HealthAnalysisType = healthAnalysisType,
         AnalysisStartDate = startDate,
         AnalysisEndDate = endDate,
         //TODO: Add business logic for applying recommendations
         Recomendations = string.Empty
      };
         
      await Repository.Add(healthAnalysis);
      
      await _unitOfWork.SaveChangesAsync();

      return pet.PetId;
   }
   

   public async Task<HealthAnalysis> UpdateHealthAnalysis(Guid healthAnalysisId,
      DateOnly startDate, DateOnly endDate, HealthStatus healthAnalysisType, string recommendations)
   {
      var healthAnalysis = await Repository.FindById(healthAnalysisId);

      if (healthAnalysis == null)
         throw new ApiException("health analysis wasn't found",404);
      
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
}