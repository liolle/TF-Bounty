using blazor.models;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Cards.ProgramCard;

public partial class ProgramCard : ComponentBase
{
    [Parameter, EditorRequired]
    public ProgramModel? program { get; set; }

    [Parameter]
    public bool highlight { get; set; } = true;

    [CascadingParameter]
    ProgramModel? SelectedProgram {get;set;}

    public string Logo { get; private set; } = "/assets/empty-bounty.png";

    protected override void OnInitialized()
    {
        if (program is not null && program.Logo is not null &&  program.Logo != "")
        {
            Logo = program.Logo;
        }
    }

    private string GetActive(){
        if (program is not null && SelectedProgram is not null){
            if (program.Equals(SelectedProgram)){
                return "card-active";
            }
        }
        return "";
    }

}