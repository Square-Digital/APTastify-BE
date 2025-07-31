using Microsoft.AspNetCore.Mvc;

namespace AP.WebAPI.Controllers;

public interface IController<in T>
{
    Task<IActionResult> Get();
    Task<IActionResult> Post([FromBody] T value);
    Task<IActionResult> Put([FromBody] T value);
    Task<IActionResult> Delete([FromBody] T value);
    Task<IActionResult> Patch([FromBody] T value);
}