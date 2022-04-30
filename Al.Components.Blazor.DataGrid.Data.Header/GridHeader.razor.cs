using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

#nullable disable

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class GridHeader : HandRenderComponent, IDisposable
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public IColumns Columns { get; set; }

        [Parameter]
        [EditorRequired]
        public string UniqueKeyResizeArea { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();
            Columns.OnDraggableChanged += OnDraggableChangedHandler;
        }

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            Columns.OnDraggableChanged -= OnDraggableChangedHandler;
        }
    }
}
