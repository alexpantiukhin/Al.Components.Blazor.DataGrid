using Al.Components.QueryableFilterExpression;

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
        public FilterMode FilterMode { get; set; } = FilterMode.None;

        /// <summary>
        /// Фильтр применён
        /// </summary>
        public bool Applied { get; private set; }

        /// <summary>
        /// Выражение фильтра
        /// </summary>
        public FilterExpression<T>? Expression { get; private set; }

        /// <summary>
        /// Устанавливает выражение фильтра
        /// </summary>
        /// <param name="filterExpression"></param>
        public async Task SetExpression(FilterExpression<T> filterExpression)
        {
            Expression = filterExpression;

            if (OnFilterChange != null)
                await OnFilterChange.Invoke();
        }

        /// <summary>
        /// Устанавливает выражение фильтра из фильтров столбцов
        /// </summary>
        /// <param name="columns">Столбцы</param>
        public async Task SerExpressionByColumns(IEnumerable<ColumnModel<T>> columns)
        {
            var columnsFilters = columns.Where(x => x.FilterExpression != null);

            if (columnsFilters.Any())
                Expression = FilterExpression<T>.GroupAnd(
                    columns
                    .Select(x => x.FilterExpression)
                    .ToArray());
            else
                Expression = null;

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
        /// Срабатывает при изменении фильтра
        /// </summary>
        public event Func<Task> OnFilterChange;
    }
}
