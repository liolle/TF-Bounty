
using blazor.models;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Cards.ReportCard;

public partial class ReportCard : ComponentBase
{
    [Parameter, EditorRequired]
    public ReportModel? Report { get; set; }

    [Parameter]
    public bool highlight { get; set; } = true;

    [CascadingParameter]
    ReportModel? SelectedReport { get; set; }


    protected override void OnInitialized()
    {

    }

    private string GetActive()
    {
        if (Report is not null && SelectedReport is not null)
        {
            if (Report.Equals(SelectedReport))
            {
                return "card-active";
            }
        }
        return "";
    }

    private string GetStatus()
    {
        if (Report is null)
        {
            return "";
        }

        return Report.Status;
    }
}