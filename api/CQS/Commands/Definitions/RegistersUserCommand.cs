namespace api.CQS;

public class RegistersUserCommand(string oid) : ICommandDefinition
{
    public string Oid { get; set; } = oid;
}