using System.Collections.Generic;
using CrewService.Models;
using System.Linq;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrewService.Repositories
{
    public class Repository : IRepository
    {
        private List<Pilot> pilots = new() //create a list of pilots
        {};

        private List<PilotDetails> flights = new ()
        {};

        public void LoadJsonToPilot()
        {
            using (StreamReader r = new StreamReader($"Repositories/crew.json"))
            {
                string json = r.ReadToEnd();
                var PilotObject = JObject.Parse(json);
                pilots = PilotObject["Crew"].ToObject<List<Pilot>>();
            }

            if (flights.Count == 0){
                foreach (Pilot pilot in pilots)
                {
                    PilotDetails pilotdetails = new PilotDetails
                    {
                        pilotId = pilot.Id,
                        flights = {}
                    };
                    flights.Add(pilotdetails);
                }
            }
        }

        public void LoadJsonToFlight()
        {
            using (StreamReader r = new StreamReader($"Repositories/flights_repo.json"))
            {
                string json = r.ReadToEnd();
                var pilotdetails = JArray.Parse(json);
                foreach (JObject obj in pilotdetails)
                {
                    flights.Find(pilot => pilot.pilotId == obj.GetValue("pilotId").ToObject<int>()).flights = obj.GetValue("flights").ToObject<List<Flights>>();
                }
            }
        }
        public void InstantiateFlightRepo()
        {
            String file_dir = "Repositories/flights_repo.json";
            if (!File.Exists(file_dir) || new FileInfo(file_dir).Length == 0) //checks if the flight repository doesn't exist as it will just keep the current "flights" variable
            {
                WriteFlightsToJson();
                return;
            }
            //write contents to flights variable
            LoadJsonToFlight();
        }

        public void WriteFlightsToJson() //private function only for reading in json files
        {
            using (StreamWriter file = File.CreateText("Repositories/flights_repo.json"))
            {
                string json = JsonConvert.SerializeObject(flights, Formatting.Indented);
                file.Write(json);
            }
        }

        public bool CheckIfWeekendValid(int pilotId, DateTime departDate, DateTime returnDate)
        {
            Pilot pilot = GetPilotID(pilotId);
            int dep_day_of_week = ((int)departDate.DayOfWeek);
            int ret_day_of_week = ((int)returnDate.DayOfWeek);

            if (pilot.WorkDays.Contains(dep_day_of_week.MapNumToWeekday()) && pilot.WorkDays.Contains(ret_day_of_week.MapNumToWeekday())) //checks if the weekend is valid
            {
                return true;
            }
            return false;
        }

        public bool CheckIfDateOverlaps(int pilotId, DateTime departDate, DateTime returnDate)
        {
            var flightlist = flights.Find( x => x.pilotId == pilotId).flights;
            if (flightlist == null) return true;
            for(int i = 0; i < flightlist.Count; i++)
                {
                    if (departDate.Subtract(flightlist[i].DepDateTime) > TimeSpan.Zero 
                    && departDate.Subtract(flightlist[i].ReturnDateTime) > TimeSpan.Zero 
                    && returnDate.Subtract(flightlist[i].DepDateTime) > TimeSpan.Zero
                    && returnDate.Subtract(flightlist[i].ReturnDateTime) > TimeSpan.Zero)
                    {
                        continue;
                    }
                    else if (departDate.Subtract(flightlist[i].DepDateTime) < TimeSpan.Zero 
                    && departDate.Subtract(flightlist[i].ReturnDateTime) < TimeSpan.Zero 
                    && returnDate.Subtract(flightlist[i].DepDateTime) < TimeSpan.Zero
                    && returnDate.Subtract(flightlist[i].ReturnDateTime) < TimeSpan.Zero)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true; //this only goes here when it has gone through all the dates and seen that they do not overlap
        }

        //2 get methods for getting items in repo
        public IEnumerable<Pilot> GetAllPilots()
        {
            return pilots;
        }

        public IEnumerable<PilotDetails> GetAllFlights()
        {
            return flights;
        }

        public Pilot GetPilot(String location, DateTime departDate, DateTime returnDate)
        {
            //check for pilots with available locations
            List<Pilot> pilot_list = pilots.Where(pilots => pilots.Base == location).ToList<Pilot>();

            if (pilot_list.Count == 0) return null;

            for(int i = 0; i < pilot_list.Count; i++)
            {
                bool checker = CheckIfDateOverlaps(pilot_list[i].Id, departDate, returnDate);
                if (checker == false)
                {
                    pilot_list.RemoveAt(i);
                }
            }

            //check for 
            int dep_day_of_week = ((int)departDate.DayOfWeek);
            int ret_day_of_week = ((int)returnDate.DayOfWeek);
            
            pilot_list = pilot_list.Where(pilot_list => pilot_list.WorkDays.Contains(dep_day_of_week.MapNumToWeekday())).ToList();
            pilot_list = pilot_list.Where(pilot_list => pilot_list.WorkDays.Contains(ret_day_of_week.MapNumToWeekday())).ToList();

            if (pilot_list.Count == 0) return null;

            pilot_list = pilot_list.OrderBy( i => Guid.NewGuid()).ToList(); //shuffles the list to ensure maximal fairness

            return pilot_list[0]; //returns the first item in the list
        }
        public Pilot GetPilotID(int id)
        {   
            return pilots.Where(pilots => pilots.Id == id).SingleOrDefault(); //return 1 item it should find or default (NULL)
        }

        public void ScheduleFlight(int pilotId, Flights flight)
        {
            for (int i = 0; i < flights.Count; i++)
            {
                if( flights[i].pilotId == pilotId)
                {
                    if (flights[i].flights == null)
                    {
                        List<Flights> f = new List<Flights>() {};
                        f.Add(flight);
                        flights[i].flights = f;
                    }
                    else{
                        flights[i].flights.Add(flight);
                    }
                    break;
                }
            }
            WriteFlightsToJson(); 
        }
    }
}