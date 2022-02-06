README

This is meant to be a RESTful service that will provide on-demand crew scheduling information to other services
This service is meant to expose 2 endpoints:
1. Request an available pilot for a given datetime and location
− The pilot must be available on the requested days at the requested location
− The pilot must not already be scheduled to fly at those dates
− Example GET
localhost/pilots/availability?location=Munich&depDateTime=2025-08-01T00:00:00Z&returnDateTime=2025-08-02T00:00:00Z

2. Schedule a Flight for a Pilot
− Pilot's slot will be reserved if available
− Example POST
localhost/flights {"pilotId": 1823, "depDateTime": "2025-08-01T09:00:00Z", "returnDateTime": "2025-08-01T10:00:00Z"}

Crew Example JSON
{ 
 "Crew": [ 
 { "ID": 1, "Name": "Andy", "Base": "Munich", "WorkDays": ["Monday", "Tuesday", "Thursday", "Saturday"] }, 
 { "ID": 2, "Name": "Betty", "Base": "Munich", "WorkDays": ["Monday", "Tuesday", "Wednesday", "Friday"] },
 { "ID": 3, "Name": "Callum", "Base": "Munich", "WorkDays": ["Wednesday", "Thursday", "Saturday", "Sunday"] }, 
 { "ID": 4, "Name": "Daphne", "Base": "Munich", "WorkDays": ["Friday", "Saturday", "Sunday"] }, 
 { "ID": 5, "Name": "Elvis", "Base": "Berlin", "WorkDays": ["Monday", "Tuesday", "Thursday", "Saturday"] }, 
 { "ID": 6, "Name": "Freida", "Base": "Berlin", "WorkDays": ["Monday", "Tuesday", "Wednesday", "Friday"] }, 
 { "ID": 7, "Name": "Greg", "Base": "Berlin", "WorkDays": ["Wednesday", "Thursday", "Saturday", "Sunday"] }, 
 { "ID": 8, "Name": "Hermione", "Base": "Berlin", "WorkDays": ["Friday", "Saturday", "Sunday"] } 
 ]
}

*** Developed in the .NET framework with C# ***

Prerequisites: 
	
* Please install .Net 5 SDK before running the files
* Please install VSCode as this project was developed in VSCode
* In VSCode please install the C# by Microsoft extension
* On VSCode press File->Open Folder open the "CrewService" folder and click (terminal has to be in the form "...Lilium-Internship-Test-2022\Crewservice> ")
* run "dotnet dev-certs https --trust" and click yes (This should get rid of the "Not secure" warning on the browser when on the swagger website)
* Please install the newtonsoft package for reading JSON files into the project
	(run "dotnet add package Newtonsoft.Json" in terminal of VSCode or powershell on the "Crewservice" directory)



Running the project:

- Run the project by pressing f5 
- For a visual website go to "https://localhost:5001/swagger/index.html"

Notes: 

- The json files can be stored in the Repository folder. Replace "crew.json" with own crew json file (The current one is the example JSON from the PDF)
- Data persistence json file is the "flights_repo.json" file which stores a list of flights corresponding to each pilot