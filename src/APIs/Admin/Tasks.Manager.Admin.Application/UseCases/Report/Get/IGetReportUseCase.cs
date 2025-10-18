namespace Tasks.Manager.Admin.Application.UseCases.Report.Get;

public interface IGetReportUseCase
{
    Task<List<GetReportResponse>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default);
}
