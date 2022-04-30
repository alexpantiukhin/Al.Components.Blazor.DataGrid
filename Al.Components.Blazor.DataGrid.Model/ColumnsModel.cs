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
    public class ColumnsModel : IColumnsNotify, IColumns
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
        public async Task DraggableChange(bool draggable, CancellationToken cancellationToken = default)
        {
            if (_draggable == draggable)
                return;

            _draggable = draggable;

            if (OnDraggableChanged != null)
                await OnDraggableChanged.Invoke(cancellationToken);
        }
        public event Func<CancellationToken, Task>? OnDraggableChanged;
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
        public OrderableDictionaryNode<string, ColumnModel>[] Visibilities => All.Where(x => x.Item.Visible).ToArray();

        /// <summary>
        /// Столбцы зафиксированные слева
        /// </summary>
        public OrderableDictionaryNode<string, ColumnModel>[] FrozenLeft =>
            All.Where(x => x.Item.FrozenType == ColumnFrozenType.Left).ToArray();

        /// <summary>
        /// Столбцы зафиксированные справа
        /// </summary>
        public OrderableDictionaryNode<string, ColumnModel>[] FrozenRight =>
            All.Where(x => x.Item.FrozenType == ColumnFrozenType.Right).ToArray();

        /// <summary>
        /// Столбцы незафиксированные
        /// </summary>
        public OrderableDictionaryNode<string, ColumnModel>[] Frozenless =>
            All.Where(x => x.Item.FrozenType == ColumnFrozenType.None).ToArray();

        public OrderableDictionaryNode<string, ColumnModel>[] Sorts => _sortColumns
            .Where(x => x.Item.Sortable && x.Item.Sort != null)
            .OrderBy(x => x.Item.SortIndex)
            .ToArray();


        /// <summary>
        /// Все столбцы
        /// </summary>
        public OrderableDictionary<string, ColumnModel> All => new();

        public double ResizerLeftPosition { get; private set; }
        #endregion


        readonly OrderableDictionary<string, ColumnModel> _sortColumns = new();


        //public void AddColumn(ColumnModel column)
        //{
        //    ParametersThrows.ThrowIsNull(column, nameof(column));

        //    _allColumns.Add(column.UniqueName, column);
        //}

        public void CompleteAddedColumns() => All.CompleteAdded();

        /// <summary>
        /// Запускает перестановку столбцов
        /// </summary>
        /// <param name="dragColumn">Перемещаемый столбец</param>
        /// <exception cref="ArgumentNullException">Возникает, если перемещаемый столбец null</exception>
        public async Task DragColumnStart(ColumnModel dragColumn, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(dragColumn, nameof(dragColumn));

            if (!Draggable)
                return;

            DraggingColumn = dragColumn;

            if (OnDragStart != null)
                await OnDragStart.Invoke(dragColumn, cancellationToken);
        }

        /// <summary>
        /// Завершает перемещение столбцов
        /// </summary>
        /// <param name="dropColumn">Столбец, радом с которым встаёт текущий</param>
        /// <param name="before"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task DragColumnEnd(ColumnModel dropColumn, bool before, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(dropColumn, nameof(dropColumn));

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
                await OnDragEnd.Invoke(DraggingColumn, cancellationToken);

            DraggingColumn = null;
        }

        /// <summary>
        /// Начать изменение размера столбца
        /// </summary>
        /// <param name="resizingColumn">Изменяемый столбец</param>
        public async Task ResizeStart(ColumnModel resizingColumn, double leftBorderHeadX, CancellationToken cancellationToken = default)
        {
            if (!resizingColumn.Resizable)
                return;

            ResizerLeftPosition = leftBorderHeadX;

            ResizingColumn = resizingColumn;

            if (OnResizeStart != null)
                await OnResizeStart.Invoke(resizingColumn, cancellationToken);
        }

        /// <summary>
        /// Завершить изменение размера столбца
        /// </summary>
        public async Task ResizeEnd()
        {
            if (ResizingColumn is null)
                return;

            if (OnResizeEnd != null)
                await OnResizeEnd.Invoke(ResizingColumn, default);

            ResizingColumn = null;
        }

        /// <summary>
        /// Изменяет ширину столбца
        /// </summary>
        /// <param name="leftBorderHeadX">Позиция по оси Х левой границы заголовка столбца</param>
        /// <param name="cursorX">Позиция курсора в данный момент</param>
        /// <returns>Возвращает позицию по X ресайзера</returns>
        public async Task<int?> Resize(double leftBorderHeadX, double cursorX, CancellationToken cancellationToken = default)
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

                await ResizingColumn.WidthChange(newWidth, cancellationToken);

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
                        await ResizingColumn.WidthChange(ColumnModel.MinWidth, cancellationToken);
                        await nextVisibleColumn.WidthChange(nextVisibleColumn.Width + freeSpace, cancellationToken);
                        return (int)leftBorderHeadX + ResizingColumn.Width;
                    }

                    // Если увеличивается, то размер соседнего не должен стать меньше минимального
                    if (nextVisibleColumn.Width - columnDiffWidth < ColumnModel.MinWidth)
                    {
                        var freeSpace = nextVisibleColumn.Width - ColumnModel.MinWidth;
                        await nextVisibleColumn.WidthChange(ColumnModel.MinWidth, cancellationToken);
                        await ResizingColumn.WidthChange(ResizingColumn.Width + freeSpace, cancellationToken);
                        return (int)leftBorderHeadX + ResizingColumn.Width;
                    }

                    await ResizingColumn.WidthChange(ResizingColumn.Width + columnDiffWidth, cancellationToken);
                    await nextVisibleColumn.WidthChange(nextVisibleColumn.Width - columnDiffWidth, cancellationToken);
                }
            }


            if (OnResizing != null)
                await OnResizing(ResizingColumn, cancellationToken);

            ResizerLeftPosition = (int)leftBorderHeadX + ResizingColumn.Width;

            return (int)leftBorderHeadX + ResizingColumn.Width;
        }

        /// <summary>
        /// Применить настройки
        /// </summary>
        /// <param name="columnsSettings">Список настроек колонок</param>
        /// <returns></returns>
        public async Task<Result> ApplySettings(ColumnsSettings columnsSettings, CancellationToken cancellationToken = default)
        {
            Result result = new();

            _draggable = columnsSettings.Draggable;
            ResizeMode = columnsSettings.ResizeMode;
            AllowResizeLastColumn = columnsSettings.AllowResizeLastColumn;

            foreach (var columnSetting in columnsSettings.Columns)
            {
                var newColumn = new ColumnModel(this, columnSetting.UniqueName);
                await newColumn.ApplySettingAsync(columnSetting, cancellationToken);

                All.Add(columnSetting.UniqueName, newColumn);
            }

            return result;
        }

        public async Task SortChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (columnModel.Sort != null)
            {
                if (!_sortColumns.HasKey(columnModel.UniqueName))
                    _sortColumns.Add(columnModel.UniqueName, columnModel);
            }
            else
                _sortColumns.Remove(columnModel.UniqueName);

            if (OnSortColumnChanged != null)
                await OnSortColumnChanged(columnModel, cancellationToken);
        }

        public async Task FrozenTypeChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (OnFixedTypeColumnChanged != null)
                await OnFixedTypeColumnChanged(columnModel, cancellationToken);
        }

        public async Task VisibleChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (OnVisibleColumnChanged != null)
                await OnVisibleColumnChanged(columnModel, cancellationToken);
        }

        public async Task FilterChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (OnFilterColumnChanged != null)
                await OnFilterColumnChanged(columnModel, cancellationToken);
        }

        public async Task SortIndexChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            ParametersThrows.ThrowIsNull(columnModel, nameof(columnModel));

            if (OnSortIndexColumnChanged != null)
                await OnSortIndexColumnChanged(columnModel, cancellationToken);
        }

        public event Func<ColumnModel, CancellationToken, Task>? OnDragStart;
        public event Func<ColumnModel, CancellationToken, Task>? OnDragEnd;
        public event Func<ColumnModel, CancellationToken, Task>? OnResizeStart;
        public event Func<ColumnModel, CancellationToken, Task>? OnResizeEnd;
        public event Func<ColumnModel, CancellationToken, Task>? OnResizing;
        public event Func<ColumnModel, CancellationToken, Task>? OnSortColumnChanged;
        public event Func<ColumnModel, CancellationToken, Task>? OnSortIndexColumnChanged;
        public event Func<ColumnModel, CancellationToken, Task>? OnFixedTypeColumnChanged;
        public event Func<ColumnModel, CancellationToken, Task>? OnVisibleColumnChanged;
        public event Func<ColumnModel, CancellationToken, Task>? OnFilterColumnChanged;
    }
}
