using api.database.entities;

namespace api.CQS;

public class GetProgramsQuery : IQueryDefinition<List<ProgramEntity>>
{
    public string? Search {get;set;}
}