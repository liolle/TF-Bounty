using api.database.entities;


namespace api.CQS;

public class GetProgramByIdQuery : IQueryDefinition<ProgramEntity>
{
    public int Id {get;set;}
}