using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Contracts.Pet;
using PetHouse.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PetController : ControllerBase
{
   private readonly IPetService _petService;

   public PetController(IPetService petService)
   {
      _petService = petService;
   }

   [HttpGet]
   [SwaggerOperation("Get all pets")]
   public async Task<IActionResult> GetAll()
   {
      var pets = await _petService.GetAll();
      return Ok(pets);
   }

   [HttpGet("{petId:Guid}")]
   [SwaggerOperation("Get pet by id")]
   public async Task<IActionResult> GetById(Guid petId)
   {
      var pet = await _petService.GetById(petId);

      return Ok(pet);
   }

   [HttpDelete("{petId:Guid}")]
   [SwaggerOperation("Delete pet by id")]
   public async Task<IActionResult> Delete(Guid petId)
   {
      var petDeletionResult = await _petService.DeleteAsync(petId);

      return Ok(petDeletionResult);
   }

   [HttpPost("{userId:Guid}")]
   [SwaggerOperation("Create pet")]
   public async Task<IActionResult> CreatePet([FromRoute] Guid userId, [FromBody] CreatePetRequest request)
   {
      var petCreationResult = await _petService.CreatePet(userId, request.PetName, request.PetBreed, request.PetWeight,
         request.CaloriesPerDay, request.ActivityLevel);
      
      return Ok(petCreationResult);
   }

   [HttpPatch("{petId:Guid}")]
   [SwaggerOperation("Update pet entity")]
   public async Task<IActionResult> UpdatePet([FromRoute] Guid petId, [FromBody] CreatePetRequest request)
   {
      var petUpdateResult = await _petService.UpdatePet(petId, request.PetName, request.PetBreed, request.PetWeight,
         request.CaloriesPerDay, request.ActivityLevel);

      return Ok(petUpdateResult);
   }


}