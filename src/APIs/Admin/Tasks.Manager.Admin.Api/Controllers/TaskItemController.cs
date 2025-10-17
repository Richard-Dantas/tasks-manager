using Microsoft.AspNetCore.Mvc;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class TaskItemController(ICreateTaskItemUseCase _createTaskUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskItemRequest request)
    {
        await _createTaskUseCase.ExecuteAsync(request);
        return NoContent();
    }
}
