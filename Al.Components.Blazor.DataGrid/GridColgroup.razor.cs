using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid
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

            DataGridModel.Columns.OnResizeEnd += OnResizeHandler;
        }


        Task OnResizeHandler(ColumnModel column, CancellationToken cancellationToken = default) => RenderAsync();

        void IDisposable.Dispose()
        {
            DataGridModel.Columns.OnResizeEnd -= OnResizeHandler;
        }
    }
}
