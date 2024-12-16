using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Contracts.Meal;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.Meal;
using Swashbuckle.AspNetCore.Annotations;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealController : ControllerBase
{
   private readonly IMealService _mealService;

   public MealController(IMealService mealService)
   {
      _mealService = mealService;
   }

   [HttpGet]
   [SwaggerOperation("Get all meals")]
   public async Task<IActionResult> GetAll()
   {
      var pets = await _mealService.GetAll();
      return Ok(pets);
   }

   [HttpGet("{mealId:Guid}")]
   [SwaggerOperation("Get meal by id")]
   public async Task<IActionResult> GetById(Guid mealId)
   {
      var meal = await _mealService.GetById(mealId);

      return Ok(meal);
   }

   [HttpDelete("{mealId:Guid}")]
   [SwaggerOperation("Delete meal by id")]
   public async Task<IActionResult> Delete(Guid mealId)
   {
      var mealDeletionResult = await _mealService.DeleteAsync(mealId);

      return Ok(mealDeletionResult);
   }

   [HttpPost("{petId:Guid}")]
   [SwaggerOperation("Add meal")]
   public async Task<IActionResult> AddMeal([FromRoute] Guid petId, [FromBody] CreateMealRequest request)
   {
      var mealCreationResult = await _mealService.AddMeal(petId, request.PortionSize, request.StartTime,
          request.AdaptiveAdjustment, request.FoodType, request.IsDaily,
         request.CalorificValue);

      return Ok(mealCreationResult);
   }

   [HttpPatch("{mealId:Guid}")]
   [SwaggerOperation("Update meal")]
   public async Task<IActionResult> UpdateMeal([FromRoute] Guid mealId, [FromBody] UpdateMealRequest request)
   {
      var mealUpdateResult = await _mealService.UpdateMeal(mealId, request.PortionSize, request.StartTime,
         request.CaloriesConsumed, request.MealStatus, request.AdaptiveAdjustment,
         request.FoodType, request.IsDaily, request.CalorificValue);

      return Ok(mealUpdateResult);
   }

   [HttpGet("pet/{petId:Guid}")]
   [SwaggerOperation("Get pet meals")]
   public async Task<IActionResult> GetMealByPetId([FromRoute] Guid petId)
   {
      var meals = await _mealService.GetByPetId(petId);

      return Ok(meals);
   }

   [HttpPatch("change-status/{mealId:Guid}")]
   [SwaggerOperation("Change meal status")]
   public async Task<IActionResult> ChangeStatus([FromRoute] Guid mealId, [FromBody] ChangeStatusRequest request)
   {
      await _mealService.ChangeStatus(mealId, request.MealStatus, request.caloriesConsumed);
      return Ok(new { Message = "Meal status successfully updated" });
   }
}