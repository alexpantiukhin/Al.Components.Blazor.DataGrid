using Al.Collections;
using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DragAndDrop;
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
                string dragPositionClass = null;

                if (DragDropHelper.IsOver && !DragDropHelper.IsDragging)
                {
                    if (LeftDragging)
                        dragPositionClass = "left";
                    else
                        dragPositionClass = "right";
                }

                string frozenClasss = null;

                if (ColumnNode.Item.FrozenType == ColumnFrozenType.Left)
                    frozenClasss = "frozen-left";
                else if (ColumnNode.Item.FrozenType == ColumnFrozenType.Right)
                    frozenClasss = "frozen-right";

                return $"column-header {(ColumnNode.Item.Sortable ? "sortable" : "")}" +
                    $" {(ColumnNode.Item.ResizeMode == ColumnResizeMode.Exactly ? "resizable" : "")}" +
                    $" {DragDropHelper.ClassList} {dragPositionClass} {frozenClasss}" +
                    $" {(anyColumnResizing ? "any-resizing" : "")}";
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
        public bool Enable => ColumnNode.Item.ResizeMode == ColumnResizeMode.Exactly;
        public double MinWidth => ColumnModel.MIN_WIDTH;
        public double MaxWidth => 0;
        public string ResizerCursorStyle => "col-resize";
        public double? StartWidth => ColumnNode.Item.Width;
        public bool StyleControl => ColumnNode.Item.FrozenType != ColumnFrozenType.None;
        public double ResizerWidth => 3;

        public bool DropAllow => DataGridModel.Columns.Draggable
            && (ColumnNode.Item.FrozenType == ColumnFrozenType.None
                || ColumnNode.Item.FrozenType == ColumnFrozenType.Left && DataGridModel.Columns.AllowFrozenLeftChanging
                || ColumnNode.Item.FrozenType == ColumnFrozenType.Right && DataGridModel.Columns.AllowFrozenRightChanging);
        public string DropEffect { get; }
        public bool DragAllow => DropAllow;
        public string DragImage { get; }
        public string OverClass => "over";
        public string DragClass => "drag";
        #endregion


        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;
        ResizeHelper ResizeHelper;
        bool anyColumnResizing = false;
        DragDropHelper DragDropHelper;
        bool LeftDragging;

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
            DragDropHelper.OnDragStart += OnDragStartHandler;
            DragDropHelper.OnDragEnd += OnDragEndHandler;
            DragDropHelper.OnDrop += OnDropHandler;
            DragDropHelper.OnDragOver += OnDragOverHandler;
            DragDropHelper.OnDragLeave += OnDragLeaveHandler;

        }

        bool firstRendered;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);


            if (firstRender)
            {
                if (ColumnNode.Item.ResizeMode != ColumnResizeMode.Auto)
                    await ResizeHelper.FirstRenderedHandler();

                firstRendered = true;

                await RenderAsync();
            }
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

        Task AnyColumnResizeStart(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            if (columnModel != ColumnNode.Item)
            {
                anyColumnResizing = true;
                return RenderAsync();
            }

            return Task.CompletedTask;
        }

        Task AnyColumnResizeEnd(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            if (columnModel != ColumnNode.Item)
            {
                anyColumnResizing = false;
                return RenderAsync();
            }

            return Task.CompletedTask;
        }

        Task OnResizeStartHandler(ResizeArgs args) => DataGridModel.Columns.ResizeStart(ColumnNode);

        async Task OnResizeEndHandler(ResizeArgs args)
        {
            await DataGridModel.Columns.ResizeEnd(args.NewWidth);
            await RenderAsync();
        }

        async Task OnDropHandler()
        {
            await DataGridModel.Columns.DragColumnEnd(ColumnNode, true);
            await RenderAsync();
        }

        async Task OnDragStartHandler()
        {
            await DataGridModel.Columns.DragColumnStart(ColumnNode.Item);
            await RenderAsync();
        }

        Task OnDragEndHandler()
        {
            return RenderAsync();
        }

        Task OnDragOverHandler(DragEventArgs args)
        {
            if ((ColumnNode.Item.Width - args.OffsetX) > (ColumnNode.Item.Width / 2))
                LeftDragging = true;
            else
                LeftDragging = false;

            return RenderAsync();
        }

        Task OnDragLeaveHandler()
        {
            return RenderAsync();
        }

        Task OnSortChangedHandler(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            return RenderAsync();
        }

        public void Dispose()
        {
            DataGridModel.Columns.OnSortColumnChanged -= OnSortChangedHandler;
            ResizeHelper.OnResizeStart -= OnResizeStartHandler;
            ResizeHelper.OnResizeEnd -= OnResizeEndHandler;
            DataGridModel.Columns.OnResizeStart -= AnyColumnResizeStart;
            DataGridModel.Columns.OnResizeEnd -= AnyColumnResizeEnd;
            DragDropHelper.OnDragStart -= OnDragStartHandler;
            DragDropHelper.OnDragEnd -= OnDragEndHandler;
            DragDropHelper.OnDrop -= OnDropHandler;
            DragDropHelper.OnDragOver -= OnDragOverHandler;
            DragDropHelper.OnDragLeave -= OnDragLeaveHandler;
        }
    }
}
