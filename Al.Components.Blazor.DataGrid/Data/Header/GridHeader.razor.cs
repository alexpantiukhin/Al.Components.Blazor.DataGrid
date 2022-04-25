using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class GridHeader<T> : HandRenderComponent
        where T : class
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

    }
}
