using Microsoft.AspNetCore.Mvc;
using Tasks.Manager.Admin.Application.UseCases.Report.Get;

namespace Tasks.Manager.Admin.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ReportController(IGetReportUseCase _getReportUseCase) : ControllerBase
{
    [HttpGet("performance/{userId:guid}")]
    public async Task<IActionResult> GetPerformanceReport(Guid userId)
    {
        var result = await _getReportUseCase.ExecuteAsync(userId);
        return Ok(result);
    }
}
