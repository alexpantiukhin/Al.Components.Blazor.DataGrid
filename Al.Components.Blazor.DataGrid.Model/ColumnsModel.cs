using Al.Collections;
using Al.Components.Blazor.DataGrid.Model.Enums;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Model

{
    /// <summary>
    /// Модель столбцов грида.
    /// <para>
    /// Столбцы добавляются 1 раз при инициализации компонента грида.<br/>
    /// Столбцы, появившиеся по какому-либо условию в разметке после инициализации
    /// к набору добавлены не будут. <br/>
    /// Управлять показом необходимо через параметр <see cref="ColumnModel{T}.Visible"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T">Тип записи грида</typeparam>
    public class ColumnsModel<T>
        where T : class
    {
        public event Func<Task> OnChangeColumns;
        public event Func<ColumnModel<T>, Task> OnDragStart;
        public event Func<Task> OnDraggableChange;
        public event Func<ColumnModel<T>, Task> OnResizeStart;


        public ColumnModel<T>[] Visibilities => All?.Where(x => x.Value.Visible).Select(x => x.Value).ToArray();
        /// <summary>
        /// Все столбцы
        /// </summary>
        public OrderableDictionary<string, ColumnModel<T>> All { get; } = new();
        public ResizeMode ResizeMode { get; set; }
        public bool Draggable { get; private set; }
        public bool AllowResizeLastColumn { get; set; }

        /// <summary>
        /// Захваченный в данный момент для перемещения столбец
        /// </summary>
        public ColumnModel<T> DraggingColumn { get; private set; }
        /// <summary>
        /// Столбец, который в данный момент меняет ширину
        /// </summary>
        public ColumnModel<T> ResizingColumn { get; private set; }

        /// <summary>
        /// Добавить столбец к набору
        /// </summary>
        /// <param name="column">Столбец</param>
        public void Add(ColumnModel<T> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            if (All.Any(x => x.Value.UniqueName == column.UniqueName))
                throw new ArgumentOutOfRangeException(nameof(column), "The column with the specified name is already in the list");

            All.Add(column.UniqueName, column);
        }


        /// <summary>
        /// Завершить формирование столбцов.<br/>
        /// Запускается сразу после инициализации компонента.
        /// </summary>
        public void FinishCreateColumns() => All.CompleteAdded();


        /// <summary>
        /// Изменяет возможность перемещения столбцов
        /// </summary>
        /// <param name="draggable">Возможно или нет</param>
        /// <param name="notify">Уведомить об изменении состояния</param>
        public async Task DraggableChange(bool draggable, bool notify)
        {
            Draggable = draggable;

            if (notify && OnDraggableChange != null)
                await OnDraggableChange.Invoke();
        }


        public async Task ReorderColumnStartHandler(ColumnModel<T> dragColumn)
        {
            DraggingColumn = dragColumn;

            if (OnDragStart != null)
                await OnDragStart.Invoke(dragColumn);
        }

        public async Task ReorderColumnEndHandler(ColumnModel<T> dropColumn, bool before)
        {
            if (DraggingColumn == dropColumn)
                return;

            if (before)
                await MoveBefore(dropColumn);
            else
                await MoveAfter(dropColumn);

            DraggingColumn = null;
        }

        /// <summary>
        /// Переставляет захваченный ранее столбец перед указанным
        /// </summary>
        /// <param name="dropColumn">Столбец, перед которым будет установлен захваченный</param>
        public async Task MoveBefore(ColumnModel<T> dropColumn)
        {
            if (DraggingColumn is null)
                return;

            var moveNode = All[DraggingColumn.UniqueName];

            if (moveNode == null) return;

            var dropNode = All[dropColumn.UniqueName];

            moveNode.MoveBefore(dropNode);

            if (OnChangeColumns != null)
                await OnChangeColumns.Invoke();
        }

        /// <summary>
        /// Переставляет захваченный ранее столбец после указанного
        /// </summary>
        /// <param name="dropColumn">Столбец, после которого будет установлен захваченный</param>
        public async Task MoveAfter(ColumnModel<T> dropColumn)
        {
            if (DraggingColumn is null)
                return;

            var moveNode = All[DraggingColumn.UniqueName];

            if (moveNode == null) return;

            var dropNode = All[dropColumn.UniqueName];

            moveNode.MoveAfter(dropNode);

            if (OnChangeColumns != null)
                await OnChangeColumns.Invoke();
        }

        public async Task ResizeStart(ColumnModel<T> resizingColumn)
        {
            ResizingColumn = resizingColumn;

            if (OnResizeStart != null)
                await OnResizeStart(resizingColumn);
        }

        public void ResizeEnd()
        {
            ResizingColumn = null;
        }

        /// <summary>
        /// Изменяет ширину столбца
        /// </summary>
        /// <param name="leftBorderHeadX">Позиция по оси Х левой границы заголовка столбца</param>
        /// <param name="cursorX">Позиция курсора в данный момент</param>
        /// <returns>Возвращает позицию по X ресайзера</returns>
        public async Task<int?> Resize(double leftBorderHeadX, double cursorX)
        {
            if (ResizingColumn == null) return null;

            var newWidth = (int)cursorX - (int)leftBorderHeadX;

            // Ширина столбца меняется за счёт размера таблицы в случае, если такой режим выбран
            // или если меняется размер последнего столбца (если такое поведение не нужно, то на
            // клиенте уберем на последнем столбце ресайзер)
            if (ResizeMode == ResizeMode.Table || ResizingColumn.NextVisible() == null)
            {

                if (newWidth < ColumnModel<T>.MinWidth)
                    await ResizingColumn.WidthChange(ColumnModel<T>.MinWidth, true);
                else
                    await ResizingColumn.WidthChange(newWidth, true);

                return (int)cursorX;
            }
            else
            {
                var columnDiffWidth = newWidth - ResizingColumn.Width;

                if (columnDiffWidth == 0)
                    return (int)cursorX;

                var nextVisibleColumn = ResizingColumn.NextVisible();

                // Если размер уменьшается, то только до минимального размера
                if (ResizingColumn.Width + columnDiffWidth < ColumnModel<T>.MinWidth)
                {
                    var freeSpace = ResizingColumn.Width - ColumnModel<T>.MinWidth;
                    await ResizingColumn.WidthChange(ColumnModel<T>.MinWidth, true);
                    await nextVisibleColumn.WidthChange(nextVisibleColumn.Width + freeSpace, true);
                    return (int)leftBorderHeadX + ResizingColumn.Width;
                }

                // Если увеличивается, то размер соседнего не должен стать меньше минимального
                if (nextVisibleColumn.Width - columnDiffWidth < ColumnModel<T>.MinWidth)
                {
                    var freeSpace = nextVisibleColumn.Width - ColumnModel<T>.MinWidth;
                    await nextVisibleColumn.WidthChange(ColumnModel<T>.MinWidth, true);
                    await ResizingColumn.WidthChange(ResizingColumn.Width + freeSpace, true);
                    return (int)leftBorderHeadX + ResizingColumn.Width;
                }

                await ResizingColumn.WidthChange(ResizingColumn.Width + columnDiffWidth, true);
                await nextVisibleColumn.WidthChange(nextVisibleColumn.Width - columnDiffWidth, true);

                return (int)cursorX;

            }
        }

    }
}
