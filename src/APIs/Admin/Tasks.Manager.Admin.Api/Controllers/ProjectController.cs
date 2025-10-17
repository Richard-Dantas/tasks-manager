using Microsoft.AspNetCore.Mvc;
using Tasks.Manager.Admin.Application.UseCases.Project.Create;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ProjectController(ICreateProjectUseCase _createProjectUseCase) : ControllerBase
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
    /// Endpoint temporário apenas para o CreatedAtAction funcionar.
    /// </summary>
    [HttpGet("{id:guid}")]
    public IActionResult GetByIdAsync(Guid id)
    {
        return Ok(new { Id = id });
    }
}
