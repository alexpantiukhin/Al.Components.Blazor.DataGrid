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

        Task OnResizeHandler(ColumnModel column, CancellationToken cancellationToken = default) => RenderAsync();

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();
        Task OnDragEndHandler(ColumnModel columnModel, CancellationToken cancellationToken = default) => RenderAsync();

        void IDisposable.Dispose()
        {
            DataGridModel.Columns.OnResizeEnd -= OnResizeHandler;
            DataGridModel.Columns.OnDraggableChanged -= OnDraggableChangedHandler;
            DataGridModel.Columns.OnDragEnd -= OnDragEndHandler;
        }
    }
}
