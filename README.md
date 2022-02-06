README

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