using Al.Components.Blazor.JsInteropExtension;
using Al.Components.Blazor.AlDataGrid.Model;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Al.Components.Blazor.HandRender;

namespace Al.Components.Blazor.AlDataGrid.Header
{
    public partial class ColumnsHeaderItem<T> : HandRenderComponent
        where T : class
    {
        [Inject]
        IJSInteropExtension JSInteropExtension { get; set; }

        [CascadingParameter]
        DataGridModel<T> GridModel { get; set; }

        /// <summary>
        /// Столбец
        /// </summary>
        [CascadingParameter]
        ColumnModel<T> Column { get; set; }

        ///// <summary>
        ///// Столбец закончили перемещать
        ///// </summary>
        //[Parameter]
        //public EventCallback OnDragEnd { get; set; }

        ///// <summary>
        ///// На столбце сбросили другую колонку
        ///// </summary>
        //[Parameter]
        //public EventCallback<bool> OnDropped { get; set; }

        //[Parameter]
        //public EventCallback OnResized { get; set; }


        string _gridTemplateColumns
        {
            get
            {
                StringBuilder result = new StringBuilder();
                if (droppingLeft == true)
                    result.Append(widthDropPlace);

                result.Append(" 1fr");

                if (Column.Sortable && Column.Sort != EnumSort.None)
                    result.Append(" auto");

                if (Column.Resizeable)
                    result.Append(" auto");

                if (droppingLeft == false)
                    result.Append($" {widthDropPlace}");

                return result.ToString();
            }
        }

        string _cssClass
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (dropping)
                    stringBuilder.Append("drop");

                if (Column.Sortable)
                    stringBuilder.Append(" sortable");

                if (Column.Dragging)
                    stringBuilder.Append(" dragging-column");

                if (Column.Draggable)
                    stringBuilder.Append(" draggable");

                if (Column.FixedType != EnumColumnFixedType.None)
                    stringBuilder.Append(" fixed");

                return stringBuilder.ToString();
            }
        }

        string _style
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (Column.FixedType == EnumColumnFixedType.Left)
                {
                    stringBuilder.Append(" left:");

                    var left = Column.ListNode.GetPreviousElements()
                        .Where(x => x.Value.Visible && x.Value.FixedType == EnumColumnFixedType.Left)
                        .Sum(x => x.Value.Width);

                    stringBuilder.Append(left);


                }

                if (Column.FixedType == EnumColumnFixedType.Left)
                {
                    stringBuilder.Append(" right:");

                    var right = Column.ListNode.GetNextElements()
                        .Where(x => x.Value.Visible && x.Value.FixedType == EnumColumnFixedType.Right)
                        .Sum(x => x.Value.Width);

                    stringBuilder.Append(right);
                }

                return stringBuilder.ToString();
            }
        }

        bool dropping;
        bool droppingLeft;
        InternalColumn<T> ColumnComponent;
        ElementReference element;
        const string widthDropPlace = "5px";
        const int widthResizer = 5;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ColumnComponent = Column.Component as InternalColumn<T>;
            GridModel.Columns.OnDraggableChange += RenderAsync;
        }

        Task HandleDragEnd()
        {
            //await GridModel.Columns.ReorderColumnEndHandler()
            //await Column.ReorderEnd();
            //dropping = false;
            //droppingLeft = null;
            //await OnDragEnd.InvokeAsync();
            return RenderAsync();
        }

        async Task HandleDragStart(DragEventArgs eventArgs)
        {
            await GridModel.Columns.ReorderColumnStartHandler(Column);
            //await Column.ReorderStart();
            //await OnDragColumnStart.InvokeAsync(Column);
            await RenderAsync();
        }

        async Task HandleDrop()
        {
            if (GridModel.Columns.DraggingColumn == null || GridModel.Columns.DraggingColumn == Column)
                return;

            await GridModel.Columns.ReorderColumnEndHandler(Column, droppingLeft);
            //if (!Dragging || Column.Columns.DraggingColumn == Column || droppingLeft == null) return;

            //dropping = false;
            //await OnDropped.InvokeAsync(droppingLeft.Value);
            //droppingLeft = null;
        }

        void HandleDragEnter()
        {
            //if (!Dragging || Column.Columns.DraggingColumn == Column) return;

            //dropping = true;

            //Refresh();
        }

        void HandleDragLeave(DragEventArgs args)
        {
            //if (!Dragging || Column.Columns.DraggingColumn == Column) return;

            //dropping = false;
            //droppingLeft = null;

            //Refresh();
        }

        async Task HandleDrag(DragEventArgs eventArgs)
        {
            //if (eventArgs.OffsetX > 150)
            //    return;

            //if (!Dragging || Column.Columns.DraggingColumn == Column) return;

            //var elementProps = await JSInteropExtension.GetElementProps(element);


            //bool previousNode = false;
            //bool nextNode = false;

            //if (Column.Columns.DraggingColumn.ListNode.Previous?.Value == Column)
            //    previousNode = true;

            //if (Column.Columns.DraggingColumn.ListNode.Next?.Value == Column)
            //    nextNode = true;

            //if (eventArgs.OffsetX <= elementProps.ClientWidth / 2)
            //{
            //    if (nextNode)
            //        droppingLeft = false;
            //    else
            //        droppingLeft = true;
            //}
            //else
            //{
            //    if (previousNode)
            //        droppingLeft = true;
            //    else
            //        droppingLeft = false;
            //}

            //Refresh();
        }

        async Task ClickSortHandler()
        {

            await GridModel.DataModel.SortHandler(Column);

            await RenderAsync();
        }


        async Task ResizeStartHandler(DragEventArgs args)
        {
            args.DataTransfer.EffectAllowed = "move";
            await GridModel.Columns.ResizeStart(Column);
            RenderAsync();
        }

        void ResizeEndHandler(DragEventArgs args)
        {
            //Column.Columns.ResizeEnd();
            //Refresh();
        }

        async Task ResizingHandler(DragEventArgs args)
        {
            //if (args.ClientX == 0)
            //{
            //    var a = 1;
            //}
            //// 0 - значит отпустили мышь вне пределов области draganddrop
            //if (Column.Columns.ResizingColumn != null && args.ClientX != 0)
            //{
            //    var headElementProps = await JSInteropExtension.GetElementProps(element);

            //    Column.Columns.Resize(headElementProps.BoundLeft, args.ClientX);

            //    await OnResized.InvokeAsync();
            //}
        }

        internal async Task DraggableColumnsChange(bool draggable)
        {
            //_draggableColumns = draggable;

            //await RefreshAsync();
        }
    }
}
