using Microsoft.AspNetCore.Mvc;
using Tasks.Manager.Admin.Application.UseCases.Project.Create;
using Tasks.Manager.Admin.Application.UseCases.Project.Delete;
using Tasks.Manager.Admin.Application.UseCases.Project.List;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ProjectController(
    ICreateProjectUseCase _createProjectUseCase,
    IListProjectUseCase _listProjectUseCase,
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
    /// Lista todos os projetos em que o usuário participa.
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <returns>Lista de projetos</returns>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<ListProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var projects = await _listProjectUseCase.ExecuteAsync(userId);

        if (projects == null || !projects.Any())
            return NotFound(new { message = "Nenhum projeto encontrado para este usuário." });

        return Ok(projects);
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
