namespace blazor.models;

public class ProgramModel
{
    public string Logo { get; }
    public int Reward { get; }
    public string Title { get; }
    public string Description { get; }

    internal ProgramModel(string logo, int reward, string title, string description)
    {
        Logo = logo;
        Reward = reward;
        Title = title;
        Description = description;
    }

    public static ProgramModel Create(string logo, int reward, string title, string description)
    {
        return new ProgramModel(logo, reward, title, description);
    }
}

public class RawProgramModel
{
    public int Id { get; set;}
    public int Creator { get; set;}
    public string Logo { get; set;} = "";
    public int Reward { get; set;}
    public required string Title { get; set;} 
    public string Description { get; set;} = "";

    public ProgramModel Extract()
    {
        return new ProgramModel(Logo, Reward, Title, Description);
    }
}

