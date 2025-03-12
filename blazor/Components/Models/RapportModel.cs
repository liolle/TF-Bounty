using System.ComponentModel.DataAnnotations;

namespace blazor.models;


public class ReportModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The title is required")]
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public int ProgramId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "";

    public ReportModel() { }

    internal ReportModel(int id, string title, string content, int programId, DateTime createdAt, string status)
    {
        Id = id;
        Title = title;
        Content = content;
        ProgramId = programId;
        CreatedAt = createdAt;
        Status = status;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return ((ReportModel)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

public class RawReportModel
{
    public int Id { get; set; }
    public int Creator { get; set; }
    public int ProgramId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public ReportModel Extract()
    {
        return new ReportModel()
        {
            Id = Id,
            Content = Content,
            ProgramId = ProgramId,
            Title = Title,
            CreatedAt = CreatedAt,
            Status = Status
        };
    }
}