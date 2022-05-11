using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;
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

        //[Parameter]
        //[EditorRequired]
        //public ResizeAreaAbstract ResizeArea { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            //ParametersThrows.ThrowIsNull(ResizeArea, nameof(ResizeArea));
            ParametersThrows.ThrowIsNull(DataGridModel, nameof(DataGridModel));

            DataGridModel.Columns.OnDraggableChanged += OnDraggableChangedHandler;
        }

        Task OnDraggableChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            DataGridModel.Columns.OnDraggableChanged -= OnDraggableChangedHandler;
        }
    }
}
