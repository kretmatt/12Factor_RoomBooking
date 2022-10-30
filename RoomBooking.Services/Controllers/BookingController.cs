using System.Net;
using Common.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.BusinessLogic.Interfaces;

namespace RoomBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController:ControllerBase
{
    private ILogger<BookingController> _logger;
    private ISimpleCRUDLogic<Booking> _bookingCRUDLogic;

    public BookingController(ISimpleCRUDLogic<Booking> bookingCrudLogic, ILogger<BookingController> logger)
    {
        _logger = logger;
        _bookingCRUDLogic = bookingCrudLogic;
    }

    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        try
        {
            _logger.LogInformation($"Received create request for a new booking");
            _bookingCRUDLogic.Create(booking);
        }
        catch (ValidationException ve)
        {
            _logger.LogError($"Booking data could not be validated successfully. Errors:{ve}");
            return BadRequest("Bad booking data");
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Data was validated, but led to other errors. Error: {e}");
            return BadRequest();
        }
        _logger.LogInformation("Booking was created");
        return StatusCode(201,booking);
    }

    [HttpGet("{id}")]
    public IActionResult Read(int id)
    {
        string errorMessage;
        try
        {
            _logger.LogInformation($"Received read request for booking {id}");
            Booking booking = _bookingCRUDLogic.ReadById(id);
            
            if (booking == null)
            {
                errorMessage = $"Booking {id} not found";
                _logger.LogError(errorMessage);
                return NotFound();
            }
                
            _logger.LogInformation($"Booking {id} was found");
            return Ok(booking);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving booking {id} led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
    
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Received request to delete a booking");
            _bookingCRUDLogic.Delete(id);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while deleting booking. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Booking was deleted");

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Booking booking)
    {
        try
        {
            _logger.LogInformation($"Received request to update a booking");

            if (id != booking.BookingId)
            {
                _logger.LogError("IDs are mismatching");
                return BadRequest();
            }
            
            _bookingCRUDLogic.Update(booking);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while updating booking. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Booking was updated");

        return Ok();
    }


    [HttpGet]
    public IActionResult ReadAll()
    {
        try
        {
            _logger.LogInformation($"Received read request for bookings");
            IEnumerable<Booking> bookings = _bookingCRUDLogic.ReadAll();
            _logger.LogInformation($"Bookings were retrieved");
            
            return Ok(bookings);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving bookings led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
}