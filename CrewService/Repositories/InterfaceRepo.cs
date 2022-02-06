using System;
using System.Collections.Generic;
using CrewService.Models;

namespace CrewService.Repositories
{
    public interface IRepository
    {
        IEnumerable<Pilot> GetAllPilots();
        IEnumerable<PilotDetails> GetAllFlights();
        Pilot GetPilotID(int id);
        Pilot GetPilot(String location, DateTime departDate, DateTime returnDate);
        bool CheckIfWeekendValid(int pilotId, DateTime departDate, DateTime returnDate);
        bool CheckIfDateOverlaps(int pilotId, DateTime departDate, DateTime returnDate);
        void LoadJsonToPilot();
        void WriteFlightsToJson();
        void InstantiateFlightRepo();
        void ScheduleFlight(int pilotId, Flights flight);
    }
}