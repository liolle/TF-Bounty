using System.ComponentModel.DataAnnotations;

namespace api.models;

public class AddRapportModel
{
    public required string Content {get;set;}
    public int ProgramId {get;set;}

    public required string Title {get;set;}
}