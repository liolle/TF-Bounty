using blazor.models;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Cards.ProgramCard;

public partial class ProgramCard : ComponentBase
{
    [Parameter,EditorRequired]
    public ProgramModel program {get;set;}

    public string Logo {get;private set;} = "/assets/empty-bounty.png";

    protected override void OnInitialized()
    {
        if (program is not null && program.Logo != ""){
            Logo = program.Logo;
        }
    }

}