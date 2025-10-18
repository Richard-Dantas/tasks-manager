namespace Tasks.Manager.Admin.Application.UseCases.Report.Get;

public class GetReportResponse
{
    public Guid UserId { get; set; }
    public int CompletedTasks { get; set; }
    public double AverageTasksPerDay { get; set; }

}
