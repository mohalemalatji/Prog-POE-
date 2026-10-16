using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Contracts;
using RaceDay.Api.Data;

namespace RaceDay.Api.Controllers;

[ApiController, Authorize, Route("api/users/profile")]
public sealed class UsersController(RaceDayDbContext db) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get the current user's profile")]
    [EndpointDescription("Returns the profile for the authenticated user.")]
    public async Task<ActionResult<UserResponse>> GetProfile()
    {
        var user = await db.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return user is null ? NotFound() : Ok(new UserResponse(user.UserID, user.FirstName, user.LastName, user.Email, user.Role));
    }


    [HttpPut]
    [EndpointSummary("Update the current user's profile")]
    [EndpointDescription("Updates the name and contact details for the authenticated user.")]
    public async Task<ActionResult<UserResponse>> UpdateProfile(ProfileUpdateRequest request)
    {
        var user = await db.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (user is null) return NotFound();
        user.FirstName = request.FirstName.Trim(); user.LastName = request.LastName.Trim();
        await db.SaveChangesAsync();
        return Ok(new UserResponse(user.UserID, user.FirstName, user.LastName, user.Email, user.Role));
    }
}
