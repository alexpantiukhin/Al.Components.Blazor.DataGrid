using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.HandRender;
using Al.Helpers.Throws;

using Microsoft.AspNetCore.Components;

#nullable disable

namespace Al.Components.Blazor.DataGrid.CommonComponents
{
    public partial class GridColgroup : HandRenderComponent, IDisposable
    {
        protected override bool HandRender => true;

        [EditorRequired]
        [Parameter]
        public DataGridModel DataGridModel { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            ParametersThrows.ThrowIsNull(DataGridModel, nameof(DataGridModel));

            DataGridModel.Columns.OnResizeEnd += OnResizeHandler;
            DataGridModel.Columns.OnDraggableChanged += OnDraggableChangedHandler;
            DataGridModel.Columns.OnDragEnd += OnDragEndHandler;
        }

        async Task OnResizeHandler(ColumnModel column, CancellationToken cancellationToken = default) => Render();

        async Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => Render();
        async Task OnDragEndHandler(ColumnModel columnModel, CancellationToken cancellationToken = default) => Render();

        void IDisposable.Dispose()
        {
            DataGridModel.Columns.OnResizeEnd -= OnResizeHandler;
            DataGridModel.Columns.OnDraggableChanged -= OnDraggableChangedHandler;
            DataGridModel.Columns.OnDragEnd -= OnDragEndHandler;
        }
    }
}
