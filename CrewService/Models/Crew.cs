using System;
using System.Collections.Generic;

namespace CrewService.Models
{
    public class Pilot //Use of record for immutable data for pilots
    {
        public int Id { get; init; } //init instead of set as suitable for immutable data
        public string Name { get; init; }

        public string Base { get; set; }

        public List<String> WorkDays { get; init; }
    }

    public record Flights //still records because flights are also considered immutable 
    {
        public Guid FlightId { get; init; }

        public DateTime DepDateTime { get; init; }

        public DateTime ReturnDateTime { get; init; }
    }

    public class PilotDetails
    {
        public int pilotId{ get; init; }

        public List<Flights> flights { get; set; }
    }
}