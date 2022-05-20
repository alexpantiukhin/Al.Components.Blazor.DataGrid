using Al.Collections;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Helpers.Throws;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель столбца грида
    /// </summary>
    public class ColumnModel : IColumn
    {
        #region Properties

        #region Visible
        bool _visible = true;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Visible { get => _visible; init => _visible = value; }
        /// <summary>
        /// Изменяет видимость
        /// </summary>
        /// <param name="visible">флаг</param>
        public async Task VisibleChange(bool visible, CancellationToken cancellationToken = default)
        {
            if (Visible == visible)
                return;

            _visible = visible;

            if (OnVisibleChanged != null)
                await OnVisibleChanged.Invoke(cancellationToken);

            await _columnsModel.VisibleChangedNotify(this, cancellationToken);
        }
        public event Func<CancellationToken, Task>? OnVisibleChanged;
        #endregion

        #region Sortable
        bool _sortable;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Sortable { get => _sortable; init => _sortable = value; }
        /// <summary>
        /// Изменяет возможность сортировки
        /// </summary>
        /// <param name="sortable"></param>
        /// <returns></returns>
        public async Task SortableChange(bool sortable, CancellationToken cancellationToken = default)
        {
            if (_sortable != sortable)
            {
                _sortable = sortable;

                if (OnSortableChanged != null)
                    await OnSortableChanged.Invoke(cancellationToken);

                if (Sort != null)
                    await SortChange(null, cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnSortableChanged;
        #endregion

        #region Width
        double _width = DefaultWidth;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public double Width { get => _width; init => _width = WidthCorrect(value); }

        /// <summary>
        /// Изменяет ширину
        /// </summary>
        /// <param name="width">Новая ширина</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public async Task WidthChange(double width, CancellationToken cancellationToken = default)
        {
            double newWidth = WidthCorrect(width);

            if (newWidth != _width)
            {
                _width = newWidth;
                if (OnWidthChanged != null)
                    await OnWidthChanged.Invoke(cancellationToken);
            }
        }

        double WidthCorrect(double value)
        {
            if (value < MinWidth) return MinWidth;
            
            if(MaxWidth != null && value > MaxWidth) return MaxWidth.Value;

            return value;
        }

        /// <summary>
        /// Срабатывает после изменения ширины столбца
        /// </summary>
        public event Func<CancellationToken, Task>? OnWidthChanged;
        #endregion

        #region Title
        string? _title;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? Title { get => _title; init => _title = value; }

        public async Task TitleChange(string? title, CancellationToken cancellationToken = default)
        {
            if (_title != title?.Trim())
            {
                _title = title;

                if (OnTitleChanged != null)
                    await OnTitleChanged.Invoke(cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnTitleChanged;
        #endregion

        #region Sort
        SortDirection? _sort;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SortDirection? Sort { get => _sort; init => _sort = value; }

        public async Task SortChange(SortDirection? sort, CancellationToken cancellationToken = default)
        {
            if (_sort != sort)
            {
                _sort = sort;

                await _columnsModel.SortChangedNotify(this, cancellationToken);

                if (OnSortChanged != null)
                    await OnSortChanged.Invoke(cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnSortChanged;
        #endregion

        #region SortIndex
        int _sortIndex;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int SortIndex { get => _sortIndex; init => _sortIndex = value; }

        public async Task SortIndexChange(int sortIndex, CancellationToken cancellationToken = default)
        {
            if (_sortIndex != sortIndex)
            {
                _sortIndex = sortIndex;

                if (OnSortIndexChanged != null)
                    await OnSortIndexChanged.Invoke(cancellationToken);

                await _columnsModel.SortIndexChangedNotify(this, cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnSortIndexChanged;
        #endregion

        #region Resizable
        bool _resizable;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Resizable { get => _resizable; init => _resizable = value; }
        public async Task ResizeableChange(bool resizeable, CancellationToken cancellationToken = default)
        {
            if (_resizable != resizeable)
            {
                _resizable = resizeable;

                if (OnResizeableChanged != null)
                    await OnResizeableChanged.Invoke(cancellationToken);
            }

        }
        public event Func<CancellationToken, Task>? OnResizeableChanged;
        #endregion

        #region FixedType
        ColumnFrozenType _fixedType = ColumnFrozenType.None;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ColumnFrozenType FrozenType { get => _fixedType; init => _fixedType = value; }
        public async Task FrozenTypeChange(ColumnFrozenType columnFixedType, CancellationToken cancellationToken = default)
        {
            if (_fixedType != columnFixedType)
            {
                _fixedType = columnFixedType;

                if (OnFrozenTypeChanged != null)
                    await OnFrozenTypeChanged.Invoke(cancellationToken);

                await _columnsModel.FrozenTypeChangedNotify(this, cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnFrozenTypeChanged;
        #endregion

        #region Filterable
        bool _filterable;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Filterable { get => _filterable; init => _filterable = value; }
        public async Task FilterableChange(bool filterable, CancellationToken cancellationToken = default)
        {
            if (_filterable != filterable)
            {
                _filterable = filterable;

                if (OnFilterableChanged != null)
                    await OnFilterableChanged.Invoke(cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnFilterableChanged;
        #endregion

        #region Filter
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public RequestFilter? Filter { get; private set; }
        public async Task FilterChange(RequestFilter? filter, CancellationToken cancellationToken = default)
        {
            if (filter != Filter)
            {
                Filter = filter;

                if (OnFilterChanged != null)
                    await OnFilterChanged.Invoke(cancellationToken);

                await _columnsModel.FilterChangedNotify(this, cancellationToken);
            }
        }
        public event Func<CancellationToken, Task>? OnFilterChanged;
        #endregion

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string UniqueName { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? HeaderComponentTypeName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? CellComponentTypeName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public double? MaxWidth { get; set; }


        #endregion


        public const int MinWidth = 50;
        public const int DefaultWidth = 130;
        private readonly IColumns _columnsModel;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="columnsModel">Модель столбцов</param>
        /// <param name="fieldOrUniqueName">Имя поля столбца или уникальное имя столбца</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданное выражение null </exception>
        public ColumnModel(IColumns columnsModel, string fieldOrUniqueName)
        {
            ParametersThrows.ThrowIsNull(columnsModel, nameof(columnsModel));
            ParametersThrows.ThrowIsWhitespace(fieldOrUniqueName, nameof(fieldOrUniqueName));
            _columnsModel = columnsModel;
            UniqueName = fieldOrUniqueName;
        }


        /// <summary>
        /// Применить пользовательские настройки
        /// </summary>
        /// <param name="settings">Настройки</param>
        public async Task ApplySettingAsync(ColumnSettings settings, CancellationToken cancellationToken = default)
        {
            var hasChange = false;

            if (_sortable != settings.Sortable)
                hasChange = true;
            if (_sort != settings.Sort)
                hasChange = true;
            if (_width != settings.Width)
                hasChange = true;
            if (_visible != settings.Visible)
                hasChange = true;
            if (_fixedType != settings.FrozenType)
                hasChange = true;
            if (Filter != settings.Filter)
                hasChange = true;
            if (_resizable != settings.Resizable)
                hasChange = true;

            _sortable = settings.Sortable;
            _sort = settings.Sort;
            _width = settings.Width;
            _visible = settings.Visible;
            _fixedType = settings.FrozenType;
            _resizable = settings.Resizable;
            Filter = settings.Filter;

            if (hasChange && OnUserSettingsChanged != null)
                await OnUserSettingsChanged.Invoke(cancellationToken);
        }


        public event Func<CancellationToken, Task>? OnUserSettingsChanged;
    }
}
