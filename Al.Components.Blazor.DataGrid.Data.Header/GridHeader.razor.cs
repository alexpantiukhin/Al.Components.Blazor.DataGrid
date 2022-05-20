using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.ResizeComponent;
using Al.Helpers.Throws;

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
        public IResizeAreaComponent ResizeArea { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            //ParametersThrows.ThrowIsNull(ResizeArea, nameof(ResizeArea));
            ParametersThrows.ThrowIsNull(DataGridModel, nameof(DataGridModel));

            DataGridModel.Columns.OnDraggableChanged += OnDraggableChangedHandler;
            DataGridModel.Columns.OnDragEnd += OnDragEndHandler;
        }

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();
        Task OnDragEndHandler(ColumnModel columnModel, CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            DataGridModel.Columns.OnDraggableChanged -= OnDraggableChangedHandler;
            DataGridModel.Columns.OnDragEnd -= OnDragEndHandler;
        }
    }
}
