namespace api.database.entities;

public class ProgramEntity
{
    public int Id { get; }
    public int Creator { get; }
    public string Logo { get; }
    public int Reward { get; }
    public string Title { get; }
    public string Description { get; }

    internal ProgramEntity(int id, int creator, string logo, int reward, string title, string description)
    {
        Id = id;
        Creator = creator;
        Logo = logo;
        Reward = reward;
        Title = title;
        Description = description;
    }

    public static ProgramEntity Create(int id, int creator, string logo, int reward, string title, string description)
    {
        return new ProgramEntity(id, creator, logo, reward, title, description);
    }
}