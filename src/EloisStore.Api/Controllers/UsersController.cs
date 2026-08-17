using EloisStore.Api.Models.Auth;
using EloisStore.Api.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/users")]
public sealed class UsersController(UsersService users) : ControllerBase
{
    [HttpGet]
    public Task<List<UserResponse>> List() => users.GetUsersAsync();
}