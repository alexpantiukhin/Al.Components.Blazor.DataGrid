using Al.Collections.QueryableFilterExpression;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель фильтра данных
    /// </summary>
    /// <typeparam name="T">Тип записи данных</typeparam>
    public class FilterModel<T>
        where T : class
    {
        #region Properties
        #region FilterMode
        FilterMode _filterMode = FilterMode.None;
        /// <summary>
        /// Режим работы фильтра
        /// </summary>
        public FilterMode FilterMode { get => _filterMode; init => _filterMode = value; }
        public async Task FilterModeChange(FilterMode filterMode)
        {
            if (_filterMode == filterMode)
                return;

            _filterMode = filterMode;

            if (OnFilterModeChanged != null)
                await OnFilterModeChanged();
        }
        public event Func<Task>? OnFilterModeChanged;
        #endregion
        /// <summary>
        /// Фильтр применён
        /// </summary>
        public bool Enabled { get; private set; } = true;

        FilterExpression<T>? _filterExpression;
        /// <summary>
        /// Выражение фильтра
        /// </summary>
        public FilterExpression<T>? Expression => Enabled ? _filterExpression : null;

        #endregion

        /// <summary>
        /// Устанавливает выражение фильтра
        /// </summary>
        /// <param name="filterExpression">Выражение</param>
        public async Task SetExpression(FilterExpression<T> filterExpression)
        {
            if (FilterMode != FilterMode.Constructor)
                return;

            _filterExpression = filterExpression;

            if (OnFilterChanged != null)
                await OnFilterChanged.Invoke();
        }


        /// <summary>
        /// Устанавливает выражение фильтра из фильтров столбцов
        /// </summary>
        /// <param name="columns">Столбцы</param>
        public async Task SetExpressionByColumns(IEnumerable<ColumnModel<T>> columns)
        {
            if (columns is null)
                throw new ArgumentNullException(nameof(columns));

            if (FilterMode != FilterMode.Row)
                return;

            FilterExpression<T>[] columnsFilters = columns
                .Where(x => x.Filter != null)
                .Select(x => x.Filter)
                .ToArray();

            if (columnsFilters.Count() > 1)
                _filterExpression = FilterExpression<T>.GroupAnd(columnsFilters);
            else if (columnsFilters.Count() == 1)
                _filterExpression = columnsFilters.FirstOrDefault();
            else
                _filterExpression = null;

            if (OnFilterChanged != null)
                await OnFilterChanged.Invoke();
        }

        /// <summary>
        /// Применяет и снимает фильтр с данных
        /// </summary>
        public async Task ToggleEnabled(bool? value = null)
        {
            if (value is not null && Enabled == value)
                return;

            Enabled = value ?? !Enabled;

            if (_filterExpression != null
                && OnFilterChanged != null)
                await OnFilterChanged.Invoke();
        }

        /// <summary>
        /// Применяет пользовательские настройки фильтра
        /// </summary>
        /// <param name="constructorExpression">выражение конструктора фильтра</param>
        /// <param name="applied">Флаг применяемости фильтра</param>
        public async void ApplySettings(FilterExpression<T>? constructorExpression, bool applied)
        {
            bool changed = _filterExpression != constructorExpression || Enabled != applied;

            _filterExpression = constructorExpression;
            Enabled = applied;

            if (changed && OnFilterChanged != null)
                await OnFilterChanged.Invoke();
        }

        /// <summary>
        /// Срабатывает при изменении фильтра
        /// </summary>
        public event Func<Task>? OnFilterChanged;
    }
}
