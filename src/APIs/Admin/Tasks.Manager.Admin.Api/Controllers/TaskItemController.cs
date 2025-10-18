using Microsoft.AspNetCore.Mvc;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.List;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Remove;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Update;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class TaskItemController(
    ICreateTaskItemUseCase _createTaskUseCase,
    IListTaskItemUseCase _listTaskItemUseCase,
    IUpdateTaskItemUseCase _updateTaskItemUseCase,
    IRemoveTaskItemUseCase _removeTaskItemUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskItemRequest request)
    {
        await _createTaskUseCase.ExecuteAsync(request);
        return NoContent();
    }

    /// <summary>
    /// Lista todas as tarefas associadas a um projeto.
    /// </summary>
    /// <param name="projectId">ID do projeto</param>
    /// <returns>Lista de tarefas</returns>
    [HttpGet("project/{projectId:guid}")]
    [ProducesResponseType(typeof(List<ListTaskItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProjectId(Guid projectId)
    {
        try
        {
            var tasks = await _listTaskItemUseCase.ExecuteAsync(projectId);
            return Ok(tasks);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza as informações de uma tarefa existente em um projeto.
    /// </summary>
    /// <param name="projectId">Identificador único do projeto ao qual a tarefa pertence.</param>
    /// <param name="taskId">Identificador único da tarefa que será atualizada.</param>
    /// <param name="request">Dados atualizados da tarefa.</param>
    /// <param name="useCase">Caso de uso responsável pela atualização da tarefa.</param>
    /// <returns>Confirmação da atualização da tarefa.</returns>
    /// <response code="200">A tarefa foi atualizada com sucesso.</response>
    /// <response code="404">O projeto ou a tarefa não foram encontrados.</response>
    [HttpPut("{projectId}/tasks/{taskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaskAsync(
    Guid projectId,
    Guid taskId,
    [FromBody] UpdateTaskItemRequest request,
    [FromServices] IUpdateTaskItemUseCase useCase)
    {
        await _updateTaskItemUseCase.ExecuteAsync(projectId, taskId, request);
        return Ok();
    }

    /// <summary>
    /// Remove uma tarefa de um projeto.
    /// </summary>
    /// <param name="projectId">ID do projeto</param>
    /// <param name="taskId">ID da tarefa</param>
    [HttpDelete("{projectId:guid}/tasks/{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
    {
        try
        {
            await _removeTaskItemUseCase.ExecuteAsync(projectId, taskId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
