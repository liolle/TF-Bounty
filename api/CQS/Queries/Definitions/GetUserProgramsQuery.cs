using api.database.entities;

namespace api.CQS;


public class GetUserProgramsQuery(string oid) : IQueryDefinition<List<ProgramEntity>>
{
    public string Oid { get; } = oid;
}