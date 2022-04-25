using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Data
{
    public partial class Resizer : HandRenderComponent, IDisposable
    {
        bool _visible;

        [EditorRequired]
        [Parameter]
        public DataGridModel DataGridModel { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DataGridModel.Columns.OnResizing += OnResizingHandler;
            DataGridModel.Columns.OnResizeStart += OnResizeStartHandler;
            DataGridModel.Columns.OnResizeEnd += OnResizeEndHandler;
        }

        Task OnResizeStartHandler(ColumnModel column, CancellationToken cancellationToken = default)
        {
            _visible = true;
            return RenderAsync();
        }

        Task OnResizeEndHandler(ColumnModel column, CancellationToken cancellationToken = default)
        {
            _visible = false;
            return RenderAsync();
        }

        Task OnResizingHandler(ColumnModel column, CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            DataGridModel.Columns.OnResizing -= OnResizingHandler;
            DataGridModel.Columns.OnResizeStart -= OnResizeStartHandler;
            DataGridModel.Columns.OnResizeEnd -= OnResizeEndHandler;
        }

    }
}
