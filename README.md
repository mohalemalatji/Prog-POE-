# RaceDay — Part 1 and Part 2

RaceDay is a local event management system for South African running, walking, and cycling events. Part 1 provides the database. Part 2 adds an ASP.NET Core REST API that uses the same local SQL Server database. The MVC user interface is planned for Part 3; Swagger is the development page used to inspect and test the API.

## Part 1 — Database

For a fresh database setup:

1. Clone this repository.
2. Open SQL Server Management Studio and connect to a local SQL Server instance.
3. Open docs/RaceDayDB.sql and execute it.

The script drops and recreates the RaceDay tables, so use it only for a fresh setup. Keep a copy of any existing data first.

Part 1 demonstration: https://youtu.be/2b8iJRbW2s4

## Part 2 — REST API

The API uses the Part 1 RaceDayDB tables on local SQL Server LocalDB. Its EF Core migrations preserve the existing tables and apply the Part 2 schema updates.

### Run locally

Requirements: Visual Studio with the .NET 10 SDK and SQL Server LocalDB.

1. Open RaceDay.sln in Visual Studio.
2. Set RaceDay.Api as the startup project and select the http launch profile.
3. For a new development setup, right-click RaceDay.Api, select Manage User Secrets, and add Jwt:Key with a value of at least 32 characters. Keep this key out of source control.
4. Press Run. In Development, the API applies EF Core migrations to the local database and opens Swagger at http://localhost:5161/swagger.

Swagger is for testing the API during development; it is not the RaceDay website.

### Roles and features

- Organiser: create and manage events and categories, link categories to events, view event enrolments, and record results.
- Participant: browse events and categories, enrol in an event category, and view personal enrolments and results.
- Both roles can register, log in, and view or update their own profile. Passwords are hashed, and login returns a JWT for protected requests.

The endpoint list and request examples are in the API endpoint plan (docs/API_Endpoint_Plan.md).

### Tests and continuous integration

The test project uses an isolated in-memory database and does not change the local RaceDayDB. In Visual Studio, open Test → Test Explorer and choose Run All. The current local run passed all three API tests.

GitHub Actions restores, builds, and tests the API on pushes and pull requests to main and part-2.

## Part 2 submission evidence

- Add a screenshot of a successful Part 2 API CI run after the workflow completes.
- Add the unlisted YouTube demonstration link after recording the Part 2 walkthrough.
- Submit this repository link on ARC.
