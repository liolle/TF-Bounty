namespace blazor.models;


public class ReportModel
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public int ProgramId { get; set; }

    public ReportModel() { }

    internal ReportModel(int id,string content, int programId)
    {
        Id = id;
        Content = content;
        ProgramId = programId;
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
    public required string Content { get; set; }
    public int ProgramId { get; set; }

    public DateTime CreatedAt { get; set; }

    public ReportModel Extract()
    {
        return new ReportModel(Id,Content, ProgramId);
    }
}