using EloisStore.Api.Models.Cart;
using EloisStore.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(UsersService UsersLogicaDoService) : ControllerBase
{
    [HttpGet]
    public Task<List<User>> List() => UsersLogicaDoService.GetUsersAsync();

}
