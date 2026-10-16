# RaceDay Part 2 REST API

Part 2 adds the ASP.NET Core REST API for the RaceDay system. The API follows the Part 1 plan, uses Entity Framework Core with the local SQL Server database, and controls access for Organiser and Participant roles. The MVC website is planned for Part 3. Swagger is used to view and try the API endpoints during development.

## Run the API locally

Requirements: Visual Studio with the .NET 10 SDK and SQL Server LocalDB.

1. Open `RaceDay.sln` in Visual Studio.
2. Set `RaceDay.Api` as the startup project and select the `http` launch profile.
3. For a new development setup, add a `Jwt:Key` value of at least 32 characters in **RaceDay.Api → Manage User Secrets**. Keep the key out of source control.
4. Press **Run**. In Development, the API applies its EF Core migrations to the local `RaceDayDB` and opens Swagger at `http://localhost:5161/swagger`.

The default connection uses `(localdb)\\MSSQLLocalDB` and the Part 1 `RaceDayDB`. The migrations update the existing database. Do not run `docs/RaceDayDB.sql` against a database that contains data: that script drops and recreates the RaceDay tables.

## Roles and features

- **Organiser:** creates, updates, and deletes events; creates categories and links them to events; views enrolments for their events; and records participants' finish times and positions.
- **Participant:** views events and categories; enrols in an event category; and views personal enrolments and results.
- **Both roles:** register, log in, and view or update their own profile. Passwords are hashed, and login returns a JWT used for protected requests.

The implemented endpoints and request details are listed in Swagger and in the [API endpoint plan](docs/API_Endpoint_Plan.md).

## Tests and GitHub Actions

In Visual Studio, open **Test → Test Explorer** and choose **Run All**. The API tests use an isolated in-memory database and do not change the local `RaceDayDB`.

The **Part 2 API CI** workflow restores, builds, and tests the project on pushes and pull requests to `main` and `part-2`. The `main` branch currently contains 20 Part 2 commits after the Part 1 baseline.

### Part 2 CI screenshot

The assessment asks for a screenshot of a successful green build in this README. Add the screenshot from a successful **Part 2 API CI** run here. The existing `successful build.png` is from the separate starter workflow, so it is not the required evidence.

## Part 2 demonstration video

[Watch the Part 2 demonstration](https://youtu.be/iiQapMxWkQs)

The assessment asks for an unlisted YouTube demonstration of the running API, code structure, endpoints, authentication, and unit tests.

## Submission

Submit this repository's GitHub link on ARC.

