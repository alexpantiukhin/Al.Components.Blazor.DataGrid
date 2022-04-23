using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Helpers.Throws;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель столбцов грида
    /// <para>
    /// Столбцы добавляются 1 раз при инициализации компонента грида.<br/>
    /// Столбцы, появившиеся по какому-либо условию в разметке после инициализации
    /// к набору добавлены не будут. <br/>
    /// Управлять показом необходимо через параметр <see cref="ColumnModel.Visible"/>
    /// </para>
    /// </summary>
    public class ColumnsModel : IColumns
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
        public ColumnModel? DraggingColumn { get; private set; }
        /// <summary>
        /// Столбец, который в данный момент меняет ширину
        /// </summary>
        public ColumnModel? ResizingColumn { get; private set; }

        /// <summary>
        /// Видимые столбцы
        /// </summary>
        public ColumnModel[] Visibilities => All.Where(x => x.Value.Visible).Select(x => x.Value).ToArray();

        readonly OrderableDictionary<string, string> _sortColumns = new ();

        /// <summary>
        /// Все столбцы
        /// </summary>
        public OrderableDictionary<string, ColumnModel> All { get; } = new();

        public double ResizerLeftPosition { get; private set; }
        #endregion

        /// <summary>
        /// Запускает перестановку столбцов
        /// </summary>
        /// <param name="dragColumn">Перемещаемый столбец</param>
        /// <exception cref="ArgumentNullException">Возникает, если перемещаемый столбец null</exception>
        public async Task DragColumnStart(ColumnModel dragColumn)
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
        public async Task DragColumnEnd(ColumnModel dropColumn, bool before)
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

            if (before)
                draggingNode.MoveBefore(dropColumn.UniqueName);
            else
                draggingNode.MoveAfter(dropColumn.UniqueName);

            if (OnDragEnd != null)
                await OnDragEnd.Invoke(DraggingColumn);

            DraggingColumn = null;
        }

        /// <summary>
        /// Начать изменение размера столбца
        /// </summary>
        /// <param name="resizingColumn">Изменяемый столбец</param>
        public async Task ResizeStart(ColumnModel resizingColumn, double leftBorderHeadX)
        {
            if (!resizingColumn.Resizable)
                return;

            ResizerLeftPosition = leftBorderHeadX;

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

            var nextVisibleNode = resizingNode.Nexts.FirstOrDefault(x => x.Item.Visible);

            if (ResizeMode == ResizeMode.Table || nextVisibleNode == null)
            {

                await ResizingColumn.WidthChange(newWidth);

            }
            else
            {
                var columnDiffWidth = newWidth - ResizingColumn.Width;

                if (columnDiffWidth != 0)
                {
                    var nextVisibleColumn = nextVisibleNode.Item;

                    // Если размер уменьшается, то только до минимального размера
                    if (ResizingColumn.Width + columnDiffWidth < ColumnModel.MinWidth)
                    {
                        var freeSpace = ResizingColumn.Width - ColumnModel.MinWidth;
                        await ResizingColumn.WidthChange(ColumnModel.MinWidth);
                        await nextVisibleColumn.WidthChange(nextVisibleColumn.Width + freeSpace);
                        return (int)leftBorderHeadX + ResizingColumn.Width;
                    }

                    // Если увеличивается, то размер соседнего не должен стать меньше минимального
                    if (nextVisibleColumn.Width - columnDiffWidth < ColumnModel.MinWidth)
                    {
                        var freeSpace = nextVisibleColumn.Width - ColumnModel.MinWidth;
                        await nextVisibleColumn.WidthChange(ColumnModel.MinWidth);
                        await ResizingColumn.WidthChange(ResizingColumn.Width + freeSpace);
                        return (int)leftBorderHeadX + ResizingColumn.Width;
                    }

                    await ResizingColumn.WidthChange(ResizingColumn.Width + columnDiffWidth);
                    await nextVisibleColumn.WidthChange(nextVisibleColumn.Width - columnDiffWidth);
                }
            }


            if (OnResizing != null)
                await OnResizing(ResizingColumn);

            ResizerLeftPosition = (int)leftBorderHeadX + ResizingColumn.Width;

            return (int)leftBorderHeadX + ResizingColumn.Width;
        }

        /// <summary>
        /// Применить настройки
        /// </summary>
        /// <param name="columnsSettings">Список настроек колонок</param>
        /// <returns></returns>
        public async Task<Result> ApplySettings(List<ColumnSettings> columnsSettings)
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

        public async Task SortChangedNotify(ColumnModel columnModel)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (columnModel.Sort != null)
            {
                if(!_sortColumns.HasKey(columnModel.UniqueName))
                    _sortColumns.Add(columnModel.UniqueName, columnModel.UniqueName);
            }
            else
                _sortColumns.Remove(columnModel.UniqueName);

            if (OnSortColumnChanged != null)
                await OnSortColumnChanged(columnModel);
        }

        public async Task FixedTypeChangedNotify(ColumnModel columnModel)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if(OnFixedTypeColumnChanged != null)
                await OnFixedTypeColumnChanged(columnModel);
        }

        public async Task VisibleChangedNotify(ColumnModel columnModel)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (OnVisibleColumnChanged != null)
                await OnVisibleColumnChanged(columnModel);
        }

        public async Task FilterChangedNotify(ColumnModel columnModel)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (OnFilterColumnChanged != null)
                await OnFilterColumnChanged(columnModel);
        }

        public event Func<ColumnModel, Task>? OnDragStart;
        public event Func<ColumnModel, Task>? OnDragEnd;
        public event Func<ColumnModel, Task>? OnResizeStart;
        public event Func<ColumnModel, Task>? OnResizeEnd;
        public event Func<ColumnModel, Task>? OnResizing;
        public event Func<ColumnModel, Task>? OnSortColumnChanged;
        public event Func<ColumnModel, Task>? OnFixedTypeColumnChanged;
        public event Func<ColumnModel, Task>? OnVisibleColumnChanged;
        public event Func<ColumnModel, Task>? OnFilterColumnChanged;

    }
}
