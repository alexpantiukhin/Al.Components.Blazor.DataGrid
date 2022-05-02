using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.ResizeComponent;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#nullable disable

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class HeaderColumn : HandRenderComponent, IDisposable
    {
        [Parameter]
        [EditorRequired]
        public ResizeAreaAbstract ResizeArea { get; set; }


        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public OrderableDictionaryNode<string, ColumnModel> ColumnNode { get; set; }


        string GridTemplateColumns
        {
            get
            {
                var columns = "auto";

                if (ColumnNode.Item.Resizable)
                    columns += " 5px";

                if (ColumnNode.Item.Sortable && ColumnNode.Item.Sort != null)
                    columns += " 5px";

                return columns;
            }
        }

        string Class
        {
            get
            {
                return $"column-header {(ColumnNode.Item.Sortable ? "sortable" : "")} {(_isHeaderOver ? "over" : "")}";
            }
        }

        string ResizerClass => $"resizer {(_isResizerOver ? "over" : "")}";
        string ResizerStyle => $"display: {(_isResizerOver ? "block" : "none")}; width: {_resizeBorder}px;";

        Resize ResizeComponent;
        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;
        bool _isHeaderOver = false;
        const double _resizeBorder = 7;
        bool _isResizerOver = false;


        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ColumnNode.Item.HeaderComponentTypeName != null)
            {
                headerComponentType = Type.GetType(ColumnNode.Item.HeaderComponentTypeName);

                if (headerComponentType != null)
                    headerComponentParameters = new()
                    {
                        { "DataGridModel", DataGridModel },
                        { "ColumnModel", ColumnNode },
                    };
            }
            ColumnNode.Item.OnSortChanged += OnSortChangedHandler;
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

        void OnMouseMoveHeaderHandler(MouseEventArgs e)
        {
            if (DataGridModel.Columns.Draggable
                && DataGridModel.Columns.ResizingColumn == null
                && (ResizeComponent.Width - e.OffsetX) > _resizeBorder)
                _isHeaderOver = true;

        }

        void OnMouseOutHeaderHandler()
        {
            _isHeaderOver = false;
        }

        public Task OnResizeStartHandler(ResizeArgs args) => DataGridModel.Columns.ResizeStart(ColumnNode);

        public Task OnResizeEndHandler(ResizeArgs args) => DataGridModel.Columns.ResizeEnd(args.NewWidth);

        Task OnSortChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        void OnResizeBorderOverHandler()
        {
            _isResizerOver = true;
        }

        void OnResizeBorderLeaveHandler()
        {
            _isResizerOver = false;
        }

        public void Dispose()
        {
            ColumnNode.Item.OnSortChanged += OnSortChangedHandler;
        }
    }
}
