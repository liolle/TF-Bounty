namespace api.models;

public record AddProgramModel 
{

    public string? Logo { get; set; } = "";

    public required  int Reward { get; set; } 

    public required  string Title  { get; set; } 
    
    public string? Description  { get; set; } = "";
}