using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

namespace Al.Components.Blazor.DataGrid.DataComponent.Header
{
    public partial class GridHeader : HandRenderComponent
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

    }
}
