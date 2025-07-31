using AP.BusinessInterfaces.Data.User;
using AP.WebAPI.Managers;
using Microsoft.AspNetCore.Mvc;

namespace AP.WebAPI.Controllers;

[Route("[controller]")]
public class UserController(UserManager manager) : Controller, IController<User>
{
    [HttpGet]
    [Route(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var user = await manager.Get(id);
        return Ok(user);
    }
    [HttpGet]
    public Task<IActionResult> Get()
    {
        throw new NotImplementedException();
    }
    [HttpPost]
    public Task<IActionResult> Post(User value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public Task<IActionResult> Put(User value)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public Task<IActionResult> Delete(User value)
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    public Task<IActionResult> Patch(User value)
    {
        throw new NotImplementedException();
    }
}