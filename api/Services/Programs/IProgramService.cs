using api.CQS;
using api.database.entities;
namespace api.services;

public interface IProgramService :
    ICommandHandler<CreateProgramCommand>,
    IQueryHandler<GetUserProgramsQuery, List<ProgramEntity>>,
    IQueryHandler<GetProgramsQuery, List<ProgramEntity>>,
    IQueryHandler<GetProgramByIdQuery, ProgramEntity>
{

}