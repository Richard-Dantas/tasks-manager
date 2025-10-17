using Microsoft.AspNetCore.Mvc;
using Tasks.Manager.Admin.Application.UseCases.Project.Create;
using Tasks.Manager.Admin.Application.UseCases.Project.Delete;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ProjectController(
    ICreateProjectUseCase _createProjectUseCase,
    IDeleteProjectUseCase _deleteProjectUseCase) : ControllerBase
{

    /// <summary>
    /// Cria um novo projeto.
    /// </summary>
    /// <param name="request">Dados do projeto</param>
    /// <returns>Id do projeto criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var projectId = await _createProjectUseCase.ExecuteAsync(request);

        return Ok(new { Id = projectId });
    }

    /// <summary>
    /// Remove um projeto, desde que não haja tarefas pendentes.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _deleteProjectUseCase.ExecuteAsync(id);
            return NoContent(); // 204 - sucesso sem conteúdo
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Endpoint temporário apenas para o CreatedAtAction funcionar.
    /// </summary>
    [HttpGet("{id:guid}")]
    public IActionResult GetByIdAsync(Guid id)
    {
        return Ok(new { Id = id });
    }
}
