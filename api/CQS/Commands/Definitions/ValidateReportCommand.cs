namespace api.CQS;

public class ValidateReportCommand(int reportId,string state) : ICommandDefinition
{
    public string State { get; set; } = state;
    public int ReportId {get;set;} = reportId;
}