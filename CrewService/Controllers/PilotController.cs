using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CrewService.DTO;
using CrewService.Models;
using CrewService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CrewService.Controllers
{
    [ApiController] //marking this class as an apicontroller to make it easier to use
    [Route("")] //declaring the http route
    public class PilotsController : ControllerBase //turning this into a controller class
    {
        private readonly IRepository repository; //currently not ideal as it introduces a dependency -> changed to be an interface so there's no dependency
        
        public PilotsController(IRepository repository)
        {
            this.repository = repository;
            this.repository.LoadJsonToPilot();
            this.repository.InstantiateFlightRepo();
        }

        //Used for checking database of both
        //Get /pilots will be passed to this method
        // [HttpGet]
        // public IEnumerable<PilotDTO> GetAllPilots() //return an ienumerable of pilots
        // {
        //     var pilots = repository.GetAllPilots().Select( pilot => pilot.PilotAsDto());
        //     return pilots;
        // }

        // [HttpGet("flights")]
        // public IEnumerable<PilotDetailsDTO> GetAllFlights()
        // {
        //     var flights = repository.GetAllFlights().Select( flights => flights.PilotDetailsAsDTO());
        //     return flights;
        // }
        
        // GET /pilots/availability
        [HttpGet("pilots/availability")]
        public ActionResult<PilotDTO> GetPilot(String location, String depDateTime, String returnDateTime)
        {
            DateTime depDate;
            DateTime returnDate;

            if (!DateTime.TryParseExact(depDateTime, "yyyy-MM-ddTHH:mm:ss%K", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out depDate))
            {
                return BadRequest($"Invalid {nameof(depDateTime)} format");
            }
            if (!DateTime.TryParseExact(returnDateTime, "yyyy-MM-ddTHH:mm:ss%K", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out returnDate))
            {
                return BadRequest($"Invalid {nameof(returnDateTime)} format");
            }

            if (returnDate.Subtract(depDate) <= TimeSpan.Zero) //simple check to see if the return time and depart time is valid
            {
                Console.WriteLine(returnDate.Subtract(depDate));
                return BadRequest($"Invalid {nameof(depDate)} and {nameof(returnDate)} difference");
            }

            var pilot = repository.GetPilot(location, depDate, returnDate); //gets a pilot available

            if (pilot == null)
            {
                return Ok("No pilots available");
            }
            //call function to update pilot time
            return Ok(pilot.PilotAsDto());
        }


        // Post /
        [HttpPost("flights")]
        public ActionResult<FlightDTO> ScheduleFlight(ScheduleFlightDTO FlightDTO)
        {
            //check if pilot id is valid
            var pilotid = FlightDTO.PilotId;
            if (repository.GetPilotID(pilotid) == null){
                return NotFound($"{nameof(pilotid)} is not found");
            }

            DateTime depDateTime;
            DateTime returnDateTime;

            if (!DateTime.TryParseExact(FlightDTO.DepDateTime, "yyyy-MM-ddTHH:mm:ss%K", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out depDateTime))
            {
                return BadRequest($"Invalid {nameof(depDateTime)} format");
            }
            if (!DateTime.TryParseExact(FlightDTO.ReturnDateTime, "yyyy-MM-ddTHH:mm:ss%K", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out returnDateTime))
            {
                return BadRequest($"Invalid {nameof(returnDateTime)} format");
            }

            if (returnDateTime.Subtract(depDateTime) <= TimeSpan.Zero) //simple check to see if the return time and depart time is valid
            {
                return BadRequest($"Invalid {nameof(depDateTime)} and {nameof(returnDateTime)} difference");
            }

            //check if both dates correspond to the correct working days
            if(!repository.CheckIfWeekendValid(pilotid, depDateTime, returnDateTime))
            {
                return BadRequest($"Invalid {nameof(depDateTime)} and {nameof(returnDateTime)} not the correct workdays");
            }

            if(!repository.CheckIfDateOverlaps(pilotid, depDateTime, returnDateTime))
            {
                return BadRequest($"Invalid {nameof(depDateTime)} and {nameof(returnDateTime)} dates overlap");
            }

            Flights flight = new()
            {
                FlightId = Guid.NewGuid(),
                DepDateTime = depDateTime,
                ReturnDateTime = returnDateTime
            };

            repository.ScheduleFlight(pilotid, flight);

            return CreatedAtAction(nameof(ScheduleFlight), new { id = flight.FlightId}, flight.FlightAsDto());
        }
    }
}