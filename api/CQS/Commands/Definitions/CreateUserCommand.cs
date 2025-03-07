namespace api.CQS;

public class CreateUserCommand(string oid) : ICommandDefinition
{
    public string Oid { get; set; } = oid;
}