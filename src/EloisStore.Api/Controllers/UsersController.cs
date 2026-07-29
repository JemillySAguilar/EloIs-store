using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(UsersService UsersLogicaDoService) : ControllerBase
{
    [HttpGet]
    public Task<List<Category>> List() => UsersLogicaDoService.ListAsync();

}
