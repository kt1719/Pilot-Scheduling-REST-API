using System;
using System.Collections.Generic;
using CrewService.Models;

namespace CrewService.DTO
{
    public class PilotDTO //Use of record for immutable data for pilots
    {
        public int Id { get; init; } //init instead of set as suitable for immutable data

        public string Name { get; init; }

        public string Base { get; init; }

        public List<String> WorkDays { get; init; }
    }

    public record FlightDTO
    {
        public Guid FlightID { get; init; }

        public DateTime DepDateTime { get; init; }

        public DateTime ReturnDateTime { get; init; }

        // public DateTimeOffset CreatedDate { get; init; }
    }

    public record ScheduleFlightDTO
    {
        public int PilotId { get; init; }
        public String DepDateTime { get; init; }

        public String ReturnDateTime { get; init; }
    }

    public class PilotDetailsDTO
    {
        public int pilotId{ get; init; }

        public List<Flights> flights { get; set; }
    }
}