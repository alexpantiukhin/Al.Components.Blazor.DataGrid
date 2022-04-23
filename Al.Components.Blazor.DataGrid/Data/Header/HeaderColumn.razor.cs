using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.JsInteropExtension;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class HeaderColumn<T> : HandRenderComponent, IDisposable
        where T : class
    {
        [Inject]
        IJSInteropExtension _jsInteropExtension { get; set; }

        [Parameter]
        [EditorRequired]
        public DataGridModel<T> DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public ColumnModel<T> ColumnModel {  get; set;}


        ElementReference _element;
        string _gridTemplateColumns
        {
            get
            {
                var columns = "auto";

                if (ColumnModel.Resizable)
                    columns += " 5px";

                if (ColumnModel.Sortable && ColumnModel.Sort != null)
                    columns += " 5px";

                return columns;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ColumnModel.OnSortChanged += RenderAsync;
        }


        public async Task ClickSortHandler()
        {
            SortDirection? newValue;
            if (ColumnModel.Sort is null)
                newValue = SortDirection.Ascending;
            else if (ColumnModel.Sort == SortDirection.Ascending)
                newValue = SortDirection.Descending;
            else
                newValue = null;

            await ColumnModel.SortChange(newValue);
        }

        public async Task OnResizeStartHandler(DragEventArgs args)
        {
            var headElementProps = await _jsInteropExtension.GetElementProps(_element);
            await DataGridModel.Columns.ResizeStart(ColumnModel, headElementProps.BoundLeft);
        }


        public async Task OnResizeHandler(DragEventArgs args)
        {
            if (DataGridModel.Columns.ResizingColumn != null && args.ClientX != 0)
            {
                var headElementProps = await _jsInteropExtension.GetElementProps(_element);

                await DataGridModel.Columns.Resize(headElementProps.BoundLeft, args.ClientX);
            }
        }

        public void Dispose()
        {
            ColumnModel.OnSortChanged -= RenderAsync;
        }
    }
}
