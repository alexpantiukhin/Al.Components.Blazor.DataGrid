using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

#nullable disable

namespace Al.Components.Blazor.DataGrid.CommonComponents
{
    public partial class GridColgroup : HandRenderComponent, IDisposable
    {
        protected override bool HandRender => true;

        [EditorRequired]
        [Parameter]
        public IColumns Columns { get; set; }

        [EditorRequired]
        [Parameter]
        public IRows Rows { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            Columns.OnResizeEnd += OnResizeHandler;
            Columns.OnDraggableChanged += OnDraggableChangedHandler;
        }

        Task OnResizeHandler(ColumnModel column, CancellationToken cancellationToken = default) => RenderAsync();

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        void IDisposable.Dispose()
        {
            Columns.OnResizeEnd -= OnResizeHandler;
            Columns.OnDraggableChanged -= OnDraggableChangedHandler;
        }
    }
}
