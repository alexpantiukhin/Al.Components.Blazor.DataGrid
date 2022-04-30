using Al.Components.Blazor.DataGrid.Model;
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
        public DataGridModel DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public string UniqueKeyResizeArea { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();
            DataGridModel.Columns.OnDraggableChanged += OnDraggableChangedHandler;
        }

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            DataGridModel.Columns.OnDraggableChanged -= OnDraggableChangedHandler;
        }
    }
}
