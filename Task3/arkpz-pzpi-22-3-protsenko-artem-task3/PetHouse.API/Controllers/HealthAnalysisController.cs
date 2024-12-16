using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHouse.API.Contracts.HealthAnalysis;
using PetHouse.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetHouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HealthAnalysisController : ControllerBase
{
   private readonly IHealthAnalysisService _healthAnalysisService;

   public HealthAnalysisController(IHealthAnalysisService healthAnalysisService)
   {
      _healthAnalysisService = healthAnalysisService;
   }

   [HttpGet]
   public async Task<IActionResult> GetAll()
   {
      var healthAnalyses = await _healthAnalysisService.GetAll();
      return Ok(healthAnalyses);
   }

   [HttpGet("{healthAnalysisId:Guid}")]
   [SwaggerOperation("Get health analysis by id")]
   public async Task<IActionResult> GetById(Guid healthAnalysisId)
   {
      var healthAnalysis = await _healthAnalysisService.GetById(healthAnalysisId);

      return Ok(healthAnalysis);
   }

   [HttpDelete("{healthAnalysisId:Guid}")]
   [SwaggerOperation("Delete health analyses by id")]
   public async Task<IActionResult> Delete(Guid healthAnalysisId)
   {
      var healthAnalysisDeletionResult = await _healthAnalysisService.DeleteAsync(healthAnalysisId);

      return Ok(healthAnalysisDeletionResult);
   }

   [HttpPost("{petId:Guid}")]
   [SwaggerOperation("Create health analysis")]
   public async Task<IActionResult> CreateHealthAnalysis([FromRoute] Guid petId,
      [FromBody] CreateHealthAnalysisRequest request)
   {
      var healthAnalysisCreationResult = await _healthAnalysisService.CreateHealthAnalysis(petId,
         request.StartAnalysisDate,request.EndAnalysisDate);
      
      return Ok(healthAnalysisCreationResult);
   }

   [HttpPatch("{healthAnalysisId:Guid}")]
   [SwaggerOperation("Update health analysis")]
   public async Task<IActionResult> UpdateHealthAnalysis([FromRoute] Guid healthAnalysisId,
      [FromBody] UpdateHealthAnalysisRequest request)
   {
      var healthAnalysisUpdateResult = await _healthAnalysisService.UpdateHealthAnalysis(healthAnalysisId,
         request.StartAnalysisDate,request.EndAnalysisDate,request.HealthAnalysisType,request.Recomendations);
      
      return Ok(healthAnalysisUpdateResult);
   }
}