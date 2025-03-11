namespace api.CQS;

public class CreateRapportCommand(int programId, string creator, string content) : ICommandDefinition
{
    public int ProgramId { get; } = programId;
    public string Creator { get; } = creator;
    public string Content {get;} = content;
    
}