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

        /// <summary>
        /// Показывать строку фильтров
        /// </summary>
        public FilterMode FilterMode { get; init; } = FilterMode.None;

        /// <summary>
        /// Фильтр применён
        /// </summary>
        public bool Applied { get; private set; } = true;

        FilterExpression<T>? _filterExpression;
        /// <summary>
        /// Выражение фильтра
        /// </summary>
        public FilterExpression<T>? Expression => Applied ? _filterExpression : null;

        /// <summary>
        /// Устанавливает выражение фильтра
        /// </summary>
        /// <param name="filterExpression">Выражение</param>
        public async Task SetExpression(FilterExpression<T> filterExpression)
        {
            if (FilterMode != FilterMode.Constructor)
                return;

            _filterExpression = filterExpression;

            if (OnFilterChange != null)
                await OnFilterChange.Invoke();
        }

        /// <summary>
        /// Устанавливает выражение фильтра из фильтров столбцов
        /// </summary>
        /// <param name="columns">Столбцы</param>
        public async Task SerExpressionByColumns(IEnumerable<ColumnModel<T>> columns)
        {
            if (FilterMode != FilterMode.Row)
                return;

            FilterExpression<T>[] columnsFilters = columns
                .Where(x => x.Filter != null)
                .Select(x => x.Filter)
                .ToArray();

            if (columnsFilters.Any())
                _filterExpression = FilterExpression<T>.GroupAnd(columnsFilters);
            else
                _filterExpression = null;

            if (OnFilterChange != null)
                await OnFilterChange.Invoke();
        }

        /// <summary>
        /// Применяет и снимает фильтр с данных
        /// </summary>
        public async Task ToggleApplyFilter()
        {
            Applied = !Applied;

            if (Expression != null
                && OnFilterChange != null)
                await OnFilterChange.Invoke();
        }

        /// <summary>
        /// Применяет пользовательские настройки фильтра
        /// </summary>
        /// <param name="constructorExpression">выражение конструктора фильтра</param>
        /// <param name="applied">Флаг применяемости фильтра</param>
        public async void ApplySettings(FilterExpression<T>? constructorExpression, bool applied)
        {
            bool changed = _filterExpression != constructorExpression || Applied != applied;

            _filterExpression = constructorExpression;
            Applied = applied;

            if (changed && OnFilterChange != null)
                await OnFilterChange.Invoke();
        }





        /// <summary>
        /// Срабатывает при изменении фильтра
        /// </summary>
        public event Func<Task>? OnFilterChange;
    }
}
