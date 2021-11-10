using Al.Collections;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;

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
        public bool Draggable { get => _draggable; init => _draggable = value; }

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

            if (OnDraggableChanged != null)
                await OnDraggableChanged.Invoke();
        }
        public event Func<Task>? OnDraggableChanged;
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
        /// Запускает перестановку столбцов
        /// </summary>
        /// <param name="dragColumn">Перемещаемый столбец</param>
        /// <exception cref="ArgumentNullException">Возникает, если перемещаемый столбец null</exception>
        public async Task DragColumnStart(ColumnModel<T> dragColumn)
        {
            if (dragColumn is null)
                throw new ArgumentNullException(nameof(dragColumn));

            if (!Draggable)
                return;

            DraggingColumn = dragColumn;

            if (OnDragStart != null)
                await OnDragStart.Invoke(dragColumn);
        }

        /// <summary>
        /// Завершает перемещение столбцов
        /// </summary>
        /// <param name="dropColumn">Столбец, радом с которым встаёт текущий</param>
        /// <param name="before"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task DragColumnEnd(ColumnModel<T> dropColumn, bool before)
        {
            if (dropColumn is null)
                throw new ArgumentNullException(nameof(dropColumn));

            if (DraggingColumn is null)
                return;

            if (DraggingColumn == dropColumn)
            {
                DraggingColumn = null;
                return;
            }

            var draggingNode = All[DraggingColumn.UniqueName];
            var dropNode = All[dropColumn.UniqueName];


            if (before)
                draggingNode.MoveBefore(dropNode);
            else
                draggingNode.MoveAfter(dropNode);

            if (OnDragEnd != null)
                await OnDragEnd.Invoke(DraggingColumn);

            DraggingColumn = null;
        }

        /// <summary>
        /// Начать изменение размера столбца
        /// </summary>
        /// <param name="resizingColumn">Изменяемый столбец</param>
        public async Task ResizeStart(ColumnModel<T> resizingColumn)
        {
            if (!resizingColumn.Resizable)
                return;

            ResizingColumn = resizingColumn;

            if (OnResizeStart != null)
                await OnResizeStart.Invoke(resizingColumn);
        }

        /// <summary>
        /// Завершить изменение размера столбца
        /// </summary>
        public async Task ResizeEnd()
        {
            if (ResizingColumn is null)
                return;

            if (OnResizeEnd != null)
                await OnResizeEnd.Invoke(ResizingColumn);

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

            var nextVisibleNode = resizingNode.NextVisible();

            if (ResizeMode == ResizeMode.Table || nextVisibleNode == null)
            {

                await ResizingColumn.WidthChange(newWidth);

            }
            else
            {
                var columnDiffWidth = newWidth - ResizingColumn.Width;

                if (columnDiffWidth != 0)
                {
                    var nextVisibleColumn = nextVisibleNode.Value;

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
                }
            }

            return (int)leftBorderHeadX + ResizingColumn.Width;
        }

        /// <summary>
        /// Применить настройки
        /// </summary>
        /// <param name="columnsSettings">Список настроек колонок</param>
        /// <returns></returns>
        public async Task<Result> ApplySettings(List<ColumnSettings<T>> columnsSettings)
        {
            Result result = new();

            for (int i = 0; i < columnsSettings.Count; i++)
            {
                var settingColumn = columnsSettings[i];

                var column = All.Select(x => x.Value).FirstOrDefault(x => x.UniqueName == settingColumn.UniqueName);

                if (column is null)
                    return result.AddError($"Настройки не актуальны. Столбца \"{settingColumn.UniqueName}\" нет в модели");

                await column.ApplySetting(settingColumn);

                All[column.UniqueName].MoveToIndex(i);
            }

            return result;
        }


        public event Func<ColumnModel<T>, Task>? OnDragStart;
        public event Func<ColumnModel<T>, Task>? OnDragEnd;
        public event Func<ColumnModel<T>, Task>? OnResizeStart;
        public event Func<ColumnModel<T>, Task>? OnResizeEnd;

    }
}
