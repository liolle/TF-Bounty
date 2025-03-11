namespace blazor.models;


public class RapportModel
{
    public string Content { get; set; } = "";
    public int ProgramId { get; set; }

    public RapportModel() { }

    internal RapportModel(string content, int programId)
    {
        Content = content;
        ProgramId = programId;
    }

}

public class RawRapportModel
{
    public int Id { get; set; }
    public int Creator { get; set; }
    public required string Content { get; set; }
    public int ProgramId { get; set; }

    public DateTime CreatedAt { get; set; }

    public RapportModel Extract()
    {
        return new RapportModel(Content, ProgramId);
    }
}