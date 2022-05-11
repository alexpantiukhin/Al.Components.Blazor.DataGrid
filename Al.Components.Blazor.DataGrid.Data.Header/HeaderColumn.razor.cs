using Al.Collections;
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
        protected override bool HandRender => true;

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


        string SortGridTemplateColumns
        {
            get
            {
                var columns = "auto";

                if (ColumnNode.Item.Sortable && ColumnNode.Item.Sort != null)
                    columns += " 5px";

                return columns;
            }
        }

        string Class
        {
            get
            {
                return $"column-header {(ColumnNode.Item.Sortable && !anyColumnResizing ? "sortable" : "")}" +
                    $" {(ColumnNode.Item.Resizable && !anyColumnResizing ? "resizable" : "")}";
            }
        }

        string SortClass
        {
            get
            {
                return $"sort {(ColumnNode.Item.Sort == null ? "" : ("show " + (ColumnNode.Item.Sort == SortDirection.Ascending ? "asc" : "desc")))}";
            }
        }

        string ContentSortClass
        {
            get
            {
                return $"column-header-content-sort {(ColumnNode.Item.Sort != null ? "sorting" : "")}";
            }
        }

        public ElementReference Element { get; set; }

        #region unused
        public bool Enable => ColumnNode.Item.Resizable;
        public EventCallback<ResizeArgs> OnResizeStart { get; }
        public EventCallback<ResizeArgs> OnResizing { get; }
        public EventCallback<ResizeArgs> OnResizeEnd { get; }
        public double MinWidth => ColumnModel.MinWidth;
        public double MaxWidth => 0;
        public EventCallback<double> WidthChanged { get; }
        public string ResizerCursorStyle => "col-resize";
        public double? StartWidth => ColumnNode.Item.Width;
        public bool StyleControl => false;
        public double ResizerWidth => 3;
        #endregion


        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;
        ResizeHelper ResizeHelper;
        bool anyColumnResizing = false;

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
            DataGridModel.Columns.OnResizeStart += AnyColumnResizeStart;
            DataGridModel.Columns.OnResizeEnd += AnyColumnResizeEnd;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
                await ResizeHelper.FirstRenderedHandler();
        }


        public async Task ClickSortHandler()
        {
            SortDirection? newValue;
            if (ColumnNode.Item.Sort is null)
                newValue = SortDirection.Ascending;
            else if (ColumnNode.Item.Sort == SortDirection.Ascending)
                newValue = SortDirection.Descending;
            else
                newValue = null;

            await ColumnNode.Item.SortChange(newValue);
        }

        async Task AnyColumnResizeStart(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            anyColumnResizing = true;
            await RenderAsync();
        }

        async Task AnyColumnResizeEnd(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            anyColumnResizing = false;
            await RenderAsync();
        }

        public Task OnResizeStartHandler() => DataGridModel.Columns.ResizeStart(ColumnNode);

        public Task OnResizeEndHandler(double newWidth) => DataGridModel.Columns.ResizeEnd(newWidth);

        Task OnSortChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            ColumnNode.Item.OnSortChanged -= OnSortChangedHandler;
            ResizeHelper.OnResizeStart -= OnResizeStartHandler;
            ResizeHelper.OnResizeEnd -= OnResizeEndHandler;
            DataGridModel.Columns.OnResizeStart -= AnyColumnResizeStart;
            DataGridModel.Columns.OnResizeEnd -= AnyColumnResizeEnd;
        }
    }
}
