namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель фильтра данных
    /// </summary>
    /// <typeparam name="T">Тип записи данных</typeparam>
    public class FilterModel
    {
        #region Properties
        /// <summary>
        /// Фильтр применён
        /// </summary>
        public bool Enabled { get; private set; } = true;

        RequestFilter? _filter;

        /// <summary>
        /// Выражение фильтра
        /// </summary>
        public RequestFilter? RequestFilter => Enabled ? _filter : null;
        #endregion

        /// <summary>
        /// Устанавливает выражение фильтра
        /// </summary>
        /// <param name="requestFilter">Выражение</param>
        public async Task SetExpression(RequestFilter requestFilter, CancellationToken cancellationToken = default)
        {
            _filter = requestFilter;

            if (OnFilterChanged != null)
                await OnFilterChanged.Invoke(cancellationToken);
        }


        /// <summary>
        /// Устанавливает выражение фильтра из фильтров столбцов
        /// </summary>
        /// <param name="columns">Столбцы</param>
        public async Task SetExpressionByColumns(IEnumerable<ColumnModel> columns, CancellationToken cancellationToken = default)
        {
            if (columns is null)
                throw new ArgumentNullException(nameof(columns));

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            RequestFilter[] columnsFilters = columns
                .Select(x => x.Filter)
                .Where(x => x != null)
                .ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            if (columnsFilters.Length > 1)
                _filter = RequestFilter.GroupAnd(columnsFilters);
            else if (columnsFilters.Length == 1)
                _filter = columnsFilters.FirstOrDefault();
            else
                _filter = null;

            if (OnFilterChanged != null)
                await OnFilterChanged.Invoke(cancellationToken);
        }

        /// <summary>
        /// Применяет и снимает фильтр с данных
        /// </summary>
        public async Task ToggleEnabled(bool? value = null, CancellationToken cancellationToken = default)
        {
            if (value is not null && Enabled == value)
                return;

            Enabled = value ?? !Enabled;

            if (_filter != null
                && OnFilterChanged != null)
                await OnFilterChanged.Invoke(cancellationToken);
        }

        /// <summary>
        /// Применяет пользовательские настройки фильтра
        /// </summary>
        /// <param name="constructorFilter">выражение конструктора фильтра</param>
        /// <param name="applied">Флаг применяемости фильтра</param>
        public void ApplySettings(RequestFilter? constructorFilter, bool applied, CancellationToken cancellationToken = default)
        {
            _filter = constructorFilter;
            Enabled = applied;
            // событие вызывать не нужно, т.к. применение настроек может вызвать 
            // множество событий в разным местах грида
        }

        /// <summary>
        /// Срабатывает при изменении фильтра
        /// </summary>
        public event Func<CancellationToken, Task>? OnFilterChanged;
    }
}
