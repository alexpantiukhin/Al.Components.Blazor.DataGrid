using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.Js.Helper;
using Al.Components.Blazor.Js.StyleHelper;
using Al.Components.Blazor.ResizeComponent;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

#nullable disable

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class HeaderColumn : HandRenderComponent, IResizeComponent, IDisposable
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public IJsHelper JsHelper { get; set; }

        [Inject]
        public JsStyleHelper JsStyleHelper { get; set; }

        [Parameter]
        [EditorRequired]
        public IResizeAreaComponent ResizeArea { get; set; }

        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public OrderableDictionaryNode<string, ColumnModel> ColumnNode { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        string GridTemplateColumns
        {
            get
            {
                var columns = "auto";

                if (ColumnNode.Item.Sortable && ColumnNode.Item.Sort != null)
                    columns += " 5px";

                if (ColumnNode.Item.Resizable)
                {
                    columns += $" {ResizerWidth}px";
                }

                return columns;
            }
        }

        string Class
        {
            get
            {
                return $"column-header {(ColumnNode.Item.Sortable ? "sortable" : "")}";
            }
        }
        public ElementReference Element { get; set; }

        //string ResizerClass => $"resizer {(_isResizerOver ? "over" : "")}";
        //string ResizerStyle => $"display: {(_isResizerOver ? "block" : "none")}; width: {_resizeBorder}px;";


        //Resize ResizeComponent;
        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;
        ResizeHelper ResizeHelper;
        //bool _isHeaderOver = false;
        //const double _resizeBorder = 7;
        //bool _isResizerOver = false;

        #region usless
        public bool Enable => ColumnNode.Item.Resizable;
        public EventCallback<ResizeArgs> OnResizeStart { get; }
        public EventCallback<ResizeArgs> OnResizing { get; }
        public EventCallback<ResizeArgs> OnResizeEnd { get; }
        public double MinWidth => ColumnModel.MinWidth;
        public double MaxWidth => 0;
        public EventCallback<double> WidthChanged { get; }
        public string ResizerCursorStyle => "col-resize";
        public double? StartWidth => ColumnNode.Item.Width;
        public bool StyleControl => true;
        public double ResizerWidth => 3;
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ResizeHelper = new(this);

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

            ResizeHelper.OnResizeStart += OnResizeStartHandler;
            ResizeHelper.OnResizeEnd += OnResizeEndHandler;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
                await ResizeHelper.FirstRenderedHandler();
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

        //void OnMouseMoveHeaderHandler(MouseEventArgs e)
        //{
        //    if (DataGridModel.Columns.Draggable
        //        && DataGridModel.Columns.ResizingColumn == null
        //        && (ResizeComponent.Width - e.OffsetX) > _resizeBorder)
        //        _isHeaderOver = true;

        //}

        //void OnMouseOutHeaderHandler()
        //{
        //    _isHeaderOver = false;
        //}

        public Task OnResizeStartHandler() => DataGridModel.Columns.ResizeStart(ColumnNode);

        public Task OnResizeEndHandler(double newWidth) => DataGridModel.Columns.ResizeEnd(newWidth);

        Task OnSortChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        //void OnResizeBorderOverHandler()
        //{
        //    _isResizerOver = true;
        //}

        //void OnResizeBorderLeaveHandler()
        //{
        //    _isResizerOver = false;
        //}

        public void Dispose()
        {
            ColumnNode.Item.OnSortChanged -= OnSortChangedHandler;

            ResizeHelper.OnResizeStart -= OnResizeStartHandler;
            ResizeHelper.OnResizeEnd -= OnResizeEndHandler;
        }
    }
}
