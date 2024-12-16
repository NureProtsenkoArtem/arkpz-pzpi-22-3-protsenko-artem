using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.Device;
using PetHouse.Core.Enums.Pet;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;


public class PetService : GenericService<Pet>, IPetService
{
   private readonly IUnitOfWork _unitOfWork;
   private readonly IStorageService _storageService;

   public PetService(IUnitOfWork unitOfWork,IStorageService storageService) : base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
      _storageService = storageService;
   }

   public async Task<Guid> CreatePet(Guid userId,string petName, string petBreed, double petWeight, double caloriesPerDay, ActivityLevel activityLevel)
   {
      var userRepository = _unitOfWork.Repository<User>();
      var user = await userRepository.FindById(userId);
      if (user == null)
      {
         throw new ApiException("User wasn't found", 404);
      }
      
      if (!Enum.IsDefined(typeof(ActivityLevel), activityLevel))
      {
         throw new ApiException("Invalid activity level", 400);
      }

      var pet = new Pet
      {
         PetId = Guid.NewGuid(),
         UserId = userId,
         PetType = petBreed,
         ActivityLevel = activityLevel,
         CaloriesPerDay = caloriesPerDay,
         PetName = petName,
         PetWeight = petWeight,
      };
      
      var recognizableData = QrCodeGenerator.GenerateAndSaveQrCode(pet);
      var recognizableDataPath =
         await _storageService.UploadFileAsync(pet.PetId.ToString(), 
            $"{pet.PetName}.png", recognizableData, "image/png");
      pet.RecognizableData = recognizableDataPath;
      
      await Repository.Add(pet);
      
      await _unitOfWork.SaveChangesAsync();
      
      return pet.PetId;
   }

   public async Task<Pet> UpdatePet(Guid petId, string petName, string petBreed, double petWeight, double caloriesPerDay, ActivityLevel activityLevel)
   {
      var pet = await Repository.FindById(petId);

      if (pet == null)
         throw new ApiException("Pet wasn't found", 404);

      if (!Enum.IsDefined(typeof(ActivityLevel), activityLevel))
      {
         throw new ApiException("Invalid activity level", 400);
      }
      
      pet.PetName = petName;
      pet.PetType = petBreed;
      pet.PetWeight = petWeight;
      pet.CaloriesPerDay = caloriesPerDay;
      pet.ActivityLevel = activityLevel;

      await Repository.Update(pet);
      await _unitOfWork.SaveChangesAsync();
      return pet;
   }
}