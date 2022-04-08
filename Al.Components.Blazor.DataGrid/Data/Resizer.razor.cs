using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Data
{
    public partial class Resizer<T> : HandRenderComponent, IDisposable
        where T : class
    {
        bool _visible;

        [EditorRequired]
        [Parameter]
        public DataGridModel<T> DataGridModel { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DataGridModel.Columns.OnResizing += OnResizingHandler;
            DataGridModel.Columns.OnResizeStart += OnResizeStartHandler;
            DataGridModel.Columns.OnResizeEnd += OnResizeEndHandler;
        }

        Task OnResizeStartHandler(ColumnModel<T> column)
        {
            _visible = true;
            return RenderAsync();
        }

        Task OnResizeEndHandler(ColumnModel<T> column)
        {
            _visible = false;
            return RenderAsync();
        }

        Task OnResizingHandler(ColumnModel<T> column) => RenderAsync();

        public void Dispose()
        {
            DataGridModel.Columns.OnResizing -= OnResizingHandler;
            DataGridModel.Columns.OnResizeStart -= OnResizeStartHandler;
            DataGridModel.Columns.OnResizeEnd -= OnResizeEndHandler;
        }

    }
}
