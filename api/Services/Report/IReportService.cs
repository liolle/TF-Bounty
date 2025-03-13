using api.CQS;
using api.database.entities;

namespace api.services;

public interface IReportService :
ICommandHandler<CreateRapportCommand>,
ICommandHandler<ValidateReportCommand>,
IQueryHandler<GetUserReportQuery, List<ReportEntity>>,
IQueryHandler<GetPendingReportQuery, List<ReportEntity>>
{

}