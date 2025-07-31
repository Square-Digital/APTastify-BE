using AP.BusinessInterfaces.Data.User;
using AP.WebAPI.Managers;
using Microsoft.AspNetCore.Mvc;

namespace AP.WebAPI.Controllers;

[Route("[controller]")]
public class UserController(UserManager manager) : Controller, IController<User>
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid id)
    {
        var user = await manager.Get(id);
        return Ok(user);
    }

    public Task<IActionResult> Get()
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Post(User value)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Put(User value)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Delete(User value)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Patch(User value)
    {
        throw new NotImplementedException();
    }
}