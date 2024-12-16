using System.Reflection.Metadata;
using PetHouse.Core.Enums.Pet;
using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface IPetService : IGenericService<Pet>
{
   Task<Guid> CreatePet(Guid userId,string petName, string petBreed, double petWeight, double caloriesPerDay,
      ActivityLevel activityLevel);

   Task<Pet> UpdatePet(Guid petId, string petName, string petBreed, double petWeight, double caloriesPerDay, ActivityLevel activityLevel);
}