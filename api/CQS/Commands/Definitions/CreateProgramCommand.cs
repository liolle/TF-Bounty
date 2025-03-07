namespace api.CQS;

public class CreateProgramCommand(string creator, string logo, int reward, string title, string description) : ICommandDefinition
{
    public string Creator { get; } = creator;
    public string Logo { get; } = logo;
    public int Reward { get; } = reward;
    public string Title { get; } = title;
    public string Description { get; } = description;
}