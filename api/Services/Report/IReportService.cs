using api.CQS;
using api.database.entities;

namespace api.services;

public interface IReportService :
ICommandHandler<CreateRapportCommand>,
IQueryHandler<GetUserReportQuery, List<ReportEntity>>
{

}