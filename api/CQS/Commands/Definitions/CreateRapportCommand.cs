namespace api.CQS;

public class CreateRapportCommand(string title,int programId, string creator, string content) : ICommandDefinition
{
    public int ProgramId { get; } = programId;
    public string Creator { get; } = creator;
    public string Content {get;} = content;
    public string Title {get;} = title;
    
}