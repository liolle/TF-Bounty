using api.database.entities;

namespace api.CQS;

public class GetUserReportQuery(string oid) : IQueryDefinition<List<ReportEntity>>
{
    public string Oid { get; } = oid;
}