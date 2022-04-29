using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace Al.Components.Blazor.DataGrid.DataComponent
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
            DataGridModel.Columns.OnDraggableChanged += OnDraggableChangedHandler;
        }


        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);

        //    if(firstRender)
        //    {
        //        await RenderAsync();
        //    }
        //}


        Task OnResizeHandler(ColumnModel column, CancellationToken cancellationToken = default) => RenderAsync();

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        void IDisposable.Dispose()
        {
            DataGridModel.Columns.OnResizeEnd -= OnResizeHandler;
            DataGridModel.Columns.OnDraggableChanged -= OnDraggableChangedHandler;
        }
    }
}
