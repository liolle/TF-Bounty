using System.ComponentModel.DataAnnotations;

namespace blazor.models;

public class ProgramModel
{
    public string? Logo { get; set; } = "";

    [Required(ErrorMessage = "Reward is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Reward must be a positive number.")]
    public int Reward { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title must be less than 100 characters.")]
    public string Title { get; set; } = "";

    public string? Description { get; set; } ="";

    public ProgramModel(){}

    internal ProgramModel(string logo, int reward, string title, string description)
    {
        Logo = logo;
        Reward = reward;
        Title = title;
        Description = description;
    }

}

public class RawProgramModel
{
    public int Id { get; set; }
    public int Creator { get; set; }
    public string Logo { get; set; } = "";
    public int Reward { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = "";

    public ProgramModel Extract()
    {
        return new ProgramModel(Logo, Reward, Title, Description);
    }
}

