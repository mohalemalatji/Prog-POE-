using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Contracts;
using RaceDay.Api.Data;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController, Authorize, Route("api/events")]
public sealed class EventsController(RaceDayDbContext db) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("List upcoming events")]
    [EndpointDescription("Returns upcoming events with their categories, entry fees, capacities, and enrolment counts.")]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAll()
    {
        var events = await db.Events.Where(e => e.Status == "Upcoming" && e.EventDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .Include(e => e.EventCategories).ThenInclude(ec => ec.Category)
            .Include(e => e.EventCategories).ThenInclude(ec => ec.Enrolments)
            .OrderBy(e => e.EventDate).ToListAsync();
        return Ok(events.Select(ToResponse));
    }


    [Authorize(Roles = "Organiser")]
    [HttpPost]
    [EndpointSummary("Create an event")]
    [EndpointDescription("Creates an event owned by the authenticated organiser.")]
    public async Task<ActionResult<EventResponse>> Create(EventCreateRequest request)
    {
        var item = new Event
        {
            OrganiserID = ActorId(), EventName = request.EventName.Trim(), Description = request.Description?.Trim(),
            EventDate = request.EventDate, Location = request.Location.Trim(), DistanceKm = request.DistanceKm,
            EventType = request.EventType
        };
        db.Events.Add(item); await db.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created, ToResponse(item));
    }


    [Authorize(Roles = "Organiser")]
    [HttpPut("{id}")]
    [EndpointSummary("Update an event")]
    [EndpointDescription("Updates an event owned by the authenticated organiser.")]
    public async Task<IActionResult> Update(string id, EventUpdateRequest request)
    {
        var item = await db.Events.FindAsync(id);
        if (item is null) return NotFound();
        if (item.OrganiserID != ActorId()) return Forbid();
        item.EventName = request.EventName.Trim(); item.Description = request.Description?.Trim(); item.EventDate = request.EventDate;
        item.Location = request.Location.Trim(); item.DistanceKm = request.DistanceKm; item.EventType = request.EventType; item.Status = request.Status;
        await db.SaveChangesAsync(); return Ok(ToResponse((await LoadEvent(id))!));
    }


    [Authorize(Roles = "Organiser")]
    [HttpDelete("{id}")]
    [EndpointSummary("Delete an event")]
    [EndpointDescription("Deletes an event owned by the authenticated organiser when it has no enrolments.")]
    public async Task<IActionResult> Delete(string id)
    {
        var item = await db.Events.FindAsync(id);
        if (item is null) return NotFound();
        if (item.OrganiserID != ActorId()) return Forbid();
        if (await db.Enrolments.AnyAsync(x => x.EventCategory!.EventID == id)) return Conflict(new { message = "An event with enrolments cannot be deleted." });
        db.Events.Remove(item); await db.SaveChangesAsync(); return NoContent();
    }


    [Authorize(Roles = "Organiser")]
    [HttpPost("{id}/categories")]
    [EndpointSummary("Add a category to an event")]
    [EndpointDescription("Links a category to the organiser's event with an entry fee and participant capacity.")]
    public async Task<ActionResult<EventCategoryResponse>> AddCategory(string id, EventCategoryRequest request)
    {
        var item = await db.Events.FindAsync(id);
        if (item is null) return NotFound();
        if (item.OrganiserID != ActorId()) return Forbid();
        var category = await db.Categories.FindAsync(request.CategoryID);
        if (category is null) return BadRequest(new { message = "Category does not exist." });
        if (await db.EventCategories.AnyAsync(x => x.EventID == id && x.CategoryID == request.CategoryID)) return Conflict(new { message = "This category is already linked to the event." });
        var link = new EventCategory { EventID = id, CategoryID = request.CategoryID, EntryFee = request.EntryFee, MaxParticipants = request.MaxParticipants };
        db.EventCategories.Add(link); await db.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created, new EventCategoryResponse(link.EventCategoryID, category.CategoryID, category.CategoryName, link.EntryFee, link.MaxParticipants, 0));
    }


    [Authorize(Roles = "Organiser")]
    [HttpGet("{id}/enrolments")]
    [EndpointSummary("List event enrolments")]
    [EndpointDescription("Returns participant enrolments and results for an event owned by the authenticated organiser.")]
    public async Task<ActionResult<IEnumerable<EnrolmentResponse>>> GetEventEnrolments(string id)
    {
        var item = await db.Events.FindAsync(id);
        if (item is null) return NotFound();
        if (item.OrganiserID != ActorId()) return Forbid();
        var rows = await db.Enrolments.Include(x => x.Participant).Include(x => x.Result)
            .Include(x => x.EventCategory).ThenInclude(x => x!.Category)
            .Include(x => x.EventCategory).ThenInclude(x => x!.Event)
            .Where(x => x.EventCategory!.EventID == id).OrderBy(x => x.EnrolmentDate).ToListAsync();
        return Ok(rows.Select(EnrolmentsController.ToResponse));
    }


    private string ActorId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


    private Task<Event?> LoadEvent(string id) => db.Events.Include(e => e.EventCategories).ThenInclude(ec => ec.Category)
        .Include(e => e.EventCategories).ThenInclude(ec => ec.Enrolments).SingleOrDefaultAsync(e => e.EventID == id);



    private static EventResponse ToResponse(Event e) => new(e.EventID, e.EventName, e.Description, e.EventDate, e.Location, e.DistanceKm,
        e.EventType, e.Status, e.OrganiserID, e.EventCategories.Select(ec => new EventCategoryResponse(ec.EventCategoryID, ec.CategoryID,
            ec.Category?.CategoryName ?? "", ec.EntryFee, ec.MaxParticipants, ec.Enrolments.Count)).ToList());
}
