using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class HeaderColumn : HandRenderComponent, IDisposable
    {

        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public ColumnModel ColumnModel { get; set; }

        [Parameter]
        [EditorRequired]
        public string UniqueKeyResizeArea { get; set; }



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

        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ColumnModel.HeaderComponentTypeName != null)
            {
                headerComponentType = Type.GetType(ColumnModel.HeaderComponentTypeName);

                if (headerComponentType != null)
                    headerComponentParameters = new()
                    {
                        { "DataGridModel", DataGridModel },
                        { "ColumnModel", ColumnModel },
                    };
            }
            ColumnModel.OnSortChanged += OnSortChangedHandler;
        }

        async Task OnBorder()
        {
            var a = 1;
        }


        public async Task ClickSortHandler()
        {
            //SortDirection? newValue;
            //if (ColumnModel.Sort is null)
            //    newValue = SortDirection.Ascending;
            //else if (ColumnModel.Sort == SortDirection.Ascending)
            //    newValue = SortDirection.Descending;
            //else
            //    newValue = null;

            //await ColumnModel.SortChange(newValue);
        }

        public async Task OnResizeStartHandler()
        {
            //var headElementProps = await _jsInteropExtension.GetElementProps(_element);
            //await DataGridModel.Columns.ResizeStart(ColumnModel, headElementProps.BoundLeft);
        }


        public async Task OnResizeHandler(DragEventArgs args)
        {
            //if (DataGridModel.Columns.ResizingColumn != null && args.ClientX != 0)
            //{
            //    var headElementProps = await _jsInteropExtension.GetElementProps(_element);

            //    await DataGridModel.Columns.Resize(headElementProps.BoundLeft, args.ClientX);
            //}
        }

        async Task OnSortChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            ColumnModel.OnSortChanged += OnSortChangedHandler;
        }
    }
}
