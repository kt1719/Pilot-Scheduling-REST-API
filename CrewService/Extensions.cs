using System;
using CrewService.DTO;
using CrewService.Models;

namespace CrewService
{
    public static class Extensions
    {
        public static PilotDTO PilotAsDto(this Pilot pilot)
        {
            return new PilotDTO
            {
                Id = pilot.Id,
                Name = pilot.Name,
                Base = pilot.Base,
                WorkDays = pilot.WorkDays
            };
        }

        public static FlightDTO FlightAsDto(this Flights flight)
        {
            return new FlightDTO
            {
                FlightID = flight.FlightId,
                DepDateTime = flight.DepDateTime,
                ReturnDateTime = flight.ReturnDateTime
                // CreatedDate = flight.CreatedDateTime
            };
        }

        public static PilotDetailsDTO PilotDetailsAsDTO(this PilotDetails pilot)
        {
            return new PilotDetailsDTO
            {
                pilotId = pilot.pilotId,
                flights = pilot.flights
            };
        }

        public static String MapNumToWeekday(this int num)
        {
            switch(num)
            {
                case 0: return "Sunday";
                case 1: return "Monday";
                case 2: return "Tuesday";
                case 3: return "Wednesday";
                case 4: return "Thursday";
                case 5: return "Friday";
                case 6: return "Saturday";
                default: return "";
            }
        }
    }
}