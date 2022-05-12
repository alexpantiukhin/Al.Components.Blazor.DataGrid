using Al.Collections;
using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DragAndDrop;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.Js.Helper;
using Al.Components.Blazor.Js.StyleHelper;
using Al.Components.Blazor.ResizeComponent;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#nullable disable

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class HeaderColumn : HandRenderComponent, IResizeComponent, IDragDropComponent, IDisposable
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


        string Class
        {
            get
            {
                return $"column-header {(ColumnNode.Item.Sortable && !anyColumnResizing ? "sortable" : "")}" +
                    $" {(ColumnNode.Item.Resizable && !anyColumnResizing ? "resizable" : "")} {DragDropHelper.ClassList}";
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
        public double MinWidth => ColumnModel.MinWidth;
        public double MaxWidth => 0;
        public string ResizerCursorStyle => "col-resize";
        public double? StartWidth => ColumnNode.Item.Width;
        public bool StyleControl => false;
        public double ResizerWidth => 3;

        public bool DropAllow => DataGridModel.Columns.Draggable;
        public string DropEffect { get; }
        public bool DragAllow => DataGridModel.Columns.Draggable;
        public string DragImage { get; }
        public string OverClass => "over";
        public string DragClass => "drag";
        #endregion


        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;
        ResizeHelper ResizeHelper;
        bool anyColumnResizing = false;
        DragDropHelper DragDropHelper;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DragDropHelper = new(this);
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
            DataGridModel.Columns.OnSortColumnChanged += OnSortChangedHandler;
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

        public Task OnResizeStartHandler(ResizeArgs args) => DataGridModel.Columns.ResizeStart(ColumnNode);

        public Task OnResizeEndHandler(ResizeArgs args) => DataGridModel.Columns.ResizeEnd(args.NewWidth);

        Task OnSortChangedHandler(ColumnModel columnModel, CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            DataGridModel.Columns.OnSortColumnChanged -= OnSortChangedHandler;
            ResizeHelper.OnResizeStart -= OnResizeStartHandler;
            ResizeHelper.OnResizeEnd -= OnResizeEndHandler;
            DataGridModel.Columns.OnResizeStart -= AnyColumnResizeStart;
            DataGridModel.Columns.OnResizeEnd -= AnyColumnResizeEnd;
        }
    }
}
