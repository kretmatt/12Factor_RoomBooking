using System.Net;
using Common.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.BusinessLogic.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class RoomController:ControllerBase, IDisposable
{
    private ILogger<RoomController> _logger;
    private ISimpleCRUDLogic<Room> _roomCRUDLogic;

    public RoomController(ISimpleCRUDLogic<Room> roomCrudLogic, ILogger<RoomController> logger)
    {
        _logger = logger;
        _roomCRUDLogic = roomCrudLogic;
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        try
        {
            _logger.LogInformation($"Received create request for a new room");
            _roomCRUDLogic.Create(room);
        }
        catch (ValidationException ve)
        {
            _logger.LogError($"Room data could not be validated successfully. Errors:{ve}");
            return BadRequest("Bad room data");
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Data was validated, but led to other errors. Error: {e}");
            return BadRequest();
        }
        _logger.LogInformation("Room was created");
        return StatusCode(201,room);
    }

    [HttpGet("{id}")]
    public IActionResult Read(int id)
    {
        string errorMessage;
        try
        {
            _logger.LogInformation($"Received read request for room {id}");
            Room room = _roomCRUDLogic.ReadById(id);
            
            if (room == null)
            {
                errorMessage = $"Room {id} not found";
                _logger.LogError(errorMessage);
                return NotFound();
            }
                
            _logger.LogInformation($"Room {id} was found");
            return Ok(room);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving room {id} led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
    
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Received request to delete a room");
            _roomCRUDLogic.Delete(id);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while deleting room. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Room was deleted");

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Room room)
    {
        try
        {
            _logger.LogInformation($"Received request to update a room");

            if (id != room.RoomId)
            {
                _logger.LogError("IDs are mismatching");
                return BadRequest();
            }
            
            _roomCRUDLogic.Update(room);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while updating room. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Room was updated");

        return Ok();
    }


    [HttpGet]
    public IActionResult ReadAll()
    {
        try
        {
            _logger.LogInformation($"Received read request for rooms");
            IEnumerable<Room> rooms = _roomCRUDLogic.ReadAll();
            _logger.LogInformation($"Rooms were retrieved");
            
            return Ok(rooms);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving rooms led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }

    public void Dispose()
    {
        _roomCRUDLogic.Dispose();
        GC.SuppressFinalize(this);
    }
}