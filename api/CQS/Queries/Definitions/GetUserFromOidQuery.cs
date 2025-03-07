using api.database.entities;

namespace api.CQS;


public class GetUserFromOidQuery(string oid) : IQueryDefinition<UserEntity>
{
    public string Oid { get; set; } = oid;
}