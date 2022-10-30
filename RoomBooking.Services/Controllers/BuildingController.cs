using System.Net;
using Common.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.BusinessLogic.Interfaces;

namespace RoomBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingController:ControllerBase
{
    private ILogger<BuildingController> _logger;
    private ISimpleCRUDLogic<Building> _buildingCRUDLogic;

    public BuildingController(ISimpleCRUDLogic<Building> buildingCrudLogic, ILogger<BuildingController> logger)
    {
        _logger = logger;
        _buildingCRUDLogic = buildingCrudLogic;
    }

    [HttpPost]
    public IActionResult Create(Building building)
    {
        try
        {
            _logger.LogInformation($"Received create request for a new building");
            _buildingCRUDLogic.Create(building);
        }
        catch (ValidationException ve)
        {
            _logger.LogError($"Building data could not be validated successfully. Errors:{ve}");
            return BadRequest("Bad building data");
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Data was validated, but led to other errors. Error: {e}");
            return BadRequest();
        }
        _logger.LogInformation("Building was created");
        return StatusCode(201,building);
    }

    [HttpGet("{id}")]
    public IActionResult Read(int id)
    {
        string errorMessage;
        try
        {
            _logger.LogInformation($"Received read request for building {id}");
            Building building = _buildingCRUDLogic.ReadById(id);
            
            if (building == null)
            {
                errorMessage = $"Building {id} not found";
                _logger.LogError(errorMessage);
                return NotFound();
            }
                
            _logger.LogInformation($"Building {id} was found");
            return Ok(building);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving building {id} led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
    
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Received request to delete a building");
            _buildingCRUDLogic.Delete(id);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while deleting building. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Building was deleted");

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Building building)
    {
        try
        {
            _logger.LogInformation($"Received request to update a building");

            if (id != building.BuildingId)
            {
                _logger.LogError("IDs are mismatching");
                return BadRequest();
            }
            
            _buildingCRUDLogic.Update(building);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while updating building. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Building was updated");

        return Ok();
    }


    [HttpGet]
    public IActionResult ReadAll()
    {
        try
        {
            _logger.LogInformation($"Received read request for buildings");
            IEnumerable<Building> buildings = _buildingCRUDLogic.ReadAll();
            _logger.LogInformation($"Buildings were retrieved");
            
            return Ok(buildings);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving buildings led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
}