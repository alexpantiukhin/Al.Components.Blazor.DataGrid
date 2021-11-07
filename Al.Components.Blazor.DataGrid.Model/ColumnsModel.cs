using Al.Collections;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;

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
        #region Properties

        #region Draggable
        bool _draggable = false;
        /// <summary>
        /// Возможность менять местами столбцы
        /// </summary>
        public bool Orderable { get => _draggable; init => _draggable = value; }

        /// <summary>
        /// Изменяет возможность перемещения столбцов
        /// </summary>
        /// <param name="draggable">Возможно или нет</param>
        /// <param name="notify">Уведомить об изменении состояния</param>
        public async Task DraggableChange(bool draggable)
        {
            if (_draggable == draggable)
                return;

            _draggable = draggable;

            if (OnDraggableChange != null)
                await OnDraggableChange.Invoke();
        }
        public event Func<Task>? OnDraggableChange;
        #endregion

        /// <summary>
        /// Режим изменения размера столбцов
        /// </summary>
        public ResizeMode ResizeMode { get; set; } = ResizeMode.Table;

        /// <summary>
        /// Разрешено менять размер последнего столбца
        /// </summary>
        public bool AllowResizeLastColumn { get; set; }
        /// <summary>
        /// Захваченный в данный момент для перемещения столбец
        /// </summary>
        public ColumnModel<T>? DraggingColumn { get; private set; }
        /// <summary>
        /// Столбец, который в данный момент меняет ширину
        /// </summary>
        public ColumnModel<T>? ResizingColumn { get; private set; }

        /// <summary>
        /// Видимые столбцы
        /// </summary>
        public ColumnModel<T>[] Visibilities => All.Where(x => x.Value.Visible).Select(x => x.Value).ToArray();
        /// <summary>
        /// Все столбцы
        /// </summary>
        public OrderableDictionary<string, ColumnModel<T>> All { get; } = new();


        #endregion

        /// <summary>
        /// Завершить формирование столбцов.<br/>
        /// Запускается сразу после инициализации компонента.
        /// </summary>
        public void FinishCreateColumns() => All.CompleteAdded();


        public async Task ReorderColumnStartHandler(ColumnModel<T> dragColumn)
        {
            if (!Orderable)
                return;

            DraggingColumn = dragColumn;

            if (OnOrderStart != null)
                await OnOrderStart.Invoke(dragColumn);
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

        ///// <summary>
        ///// Переставляет захваченный ранее столбец перед указанным
        ///// </summary>
        ///// <param name="dropColumn">Столбец, перед которым будет установлен захваченный</param>
        //public async Task MoveBefore(ColumnModel<T> dropColumn)
        //{
        //    if (DraggingColumn is null)
        //        return;

        //    var moveNode = All[DraggingColumn.UniqueName];

        //    if (moveNode == null) return;

        //    var dropNode = All[dropColumn.UniqueName];

        //    moveNode.MoveBefore(dropNode);

        //    if (OnColumnsOrdered != null)
        //        await OnColumnsOrdered.Invoke();
        //}

        ///// <summary>
        ///// Переставляет захваченный ранее столбец после указанного
        ///// </summary>
        ///// <param name="dropColumn">Столбец, после которого будет установлен захваченный</param>
        //public async Task MoveAfter(ColumnModel<T> dropColumn)
        //{
        //    if (DraggingColumn is null)
        //        return;

        //    var moveNode = All[DraggingColumn.UniqueName];

        //    if (moveNode == null) return;

        //    var dropNode = All[dropColumn.UniqueName];

        //    moveNode.MoveAfter(dropNode);

        //    if (OnColumnsOrdered != null)
        //        await OnColumnsOrdered.Invoke();
        //}

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

            var resizingNode = All[ResizingColumn.UniqueName];

            if (ResizeMode == ResizeMode.Table || resizingNode.NextVisible() == null)
            {

                await ResizingColumn.WidthChange(newWidth);

                return (int)cursorX;
            }
            else
            {
                var columnDiffWidth = newWidth - ResizingColumn.Width;

                if (columnDiffWidth == 0)
                    return (int)cursorX;

                var nextVisibleColumn = resizingNode.NextVisible()?.Value;

                // Если размер уменьшается, то только до минимального размера
                if (ResizingColumn.Width + columnDiffWidth < ColumnModel<T>.MinWidth)
                {
                    var freeSpace = ResizingColumn.Width - ColumnModel<T>.MinWidth;
                    await ResizingColumn.WidthChange(ColumnModel<T>.MinWidth);
                    await nextVisibleColumn.WidthChange(nextVisibleColumn.Width + freeSpace);
                    return (int)leftBorderHeadX + ResizingColumn.Width;
                }

                // Если увеличивается, то размер соседнего не должен стать меньше минимального
                if (nextVisibleColumn.Width - columnDiffWidth < ColumnModel<T>.MinWidth)
                {
                    var freeSpace = nextVisibleColumn.Width - ColumnModel<T>.MinWidth;
                    await nextVisibleColumn.WidthChange(ColumnModel<T>.MinWidth);
                    await ResizingColumn.WidthChange(ResizingColumn.Width + freeSpace);
                    return (int)leftBorderHeadX + ResizingColumn.Width;
                }

                await ResizingColumn.WidthChange(ResizingColumn.Width + columnDiffWidth);
                await nextVisibleColumn.WidthChange(nextVisibleColumn.Width - columnDiffWidth);

                return (int)cursorX;

            }
        }


        public async Task<Result> ApplySettings(List<ColumnSettings<T>> columns)
        {
            Result result = new();

            if (columns.Count != All.Count)
                return result.AddError("Настройки не актуальны. Число столбцов в настройках не совпадает с их числом в модели");

            for (int i = 0; i < columns.Count; i++)
            {
                var settingColumn = columns[i];

                var column = All.Select(x => x.Value).FirstOrDefault(x => x.UniqueName == settingColumn.UniqueName);

                if (column is null)
                    return result.AddError($"Настройки не актуальны. Столбца \"{settingColumn.UniqueName}\" нет в модели");

                await column.ApplySetting(settingColumn);

                All[column.UniqueName].MoveToIndex(i);
            }

            return result;
        }


        public event Func<ColumnModel<T>, Task>? OnOrderStart;
        public event Func<ColumnModel<T>, Task>? OnOrderEnd;
        public event Func<ColumnModel<T>, Task>? OnResizeStart;
        public event Func<ColumnModel<T>, Task>? OnResizeEnd;

    }
}
