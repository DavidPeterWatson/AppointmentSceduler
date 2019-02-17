# Appointment Scheduler

The purpose of the is library is to provide functionality for scheduling appointments in a schared calendar.

## About

This project is the result of a C# test for Fixx IT, completed by David Watson.
This project was written with VS Code on a Mac.
Xunit.Gherkin.Quick was used for BDD tests instead of Specflow because Specflow does not run on Mac.

## To Test

To run the tests clone the repo and run the following commands
dotnet restore
dotnet build
dotnet test

## Brief

The Brief of the test was as follows:

Your task is to implement part of an appointment scheduling library for a company called BestMed, a small medical practice consisting of 3 different medical practitioners.

The library should provide functionality for scheduling appointments using a single shared calendar.

This calendar must keep track of all appointments made on behalf of clients with the different medical practitioners. An appointment must specify the client, the medical practitioner, the timeslot and a brief description on the reason for the appointment.

No scheduling conflicts may occur and the library must provide functionality for finding the next available timeslot.

The following are assumptions and definitions that limit the scope of the task:
●	No front end is required. Only the functioning library is required.
●	The usage of the library must be specified via unit/integration tests.
●	No backing store is required, an in memory representation will suffice.
●	The goal of this test is to see whether good practices are followed, specifically:
○	SOLID design principles
○	OOP principles
○	Clean Code and/or Microsoft recommended coding standards
○	Unit testing

Submission:
●	Return deliverables as electronic source code files in reply to the Test instructions email. Please do not include any binary files.
●	Please submit all files you think are required to do your solution justice.
●	Ensure that you include the names (and versions) of any 3rd party frameworks used for testing.
●	Note: if you don’t have Visual Studio, don’t forget that the C# compiler (csc) is part of the .NET Framework installation. You may use any of the .NET framework versions from 2.0 onwards.
●	The source code file(s) delivered must compile.

As a final note, please follow the instructions and do the work to an industry standard level.
