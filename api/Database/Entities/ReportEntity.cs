namespace api.database.entities;

public class ReportEntity
{
    internal ReportEntity(int id, int creator, int programId, string title, string content, string status, DateTime createdAt)
    {
        Id = id;
        Creator = creator;
        ProgramId = programId;
        Title = title;
        Content = content;
        Status = status;
        CreatedAt = createdAt;
    }


    public int Id { get; }
    public int Creator { get; }
    public int ProgramId { get; }
    public string Title { get; }
    public string Content { get; }
    public string Status { get; }
    public DateTime CreatedAt { get; }



    public static ReportEntity Create(int id, int creator, int programId, string title, string content, string status, DateTime createdAt)
    {
        return new ReportEntity(id, creator, programId, title, content, status, createdAt);
    }
}