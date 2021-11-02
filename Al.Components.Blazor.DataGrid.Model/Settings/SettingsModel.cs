using Al.Collections.QueryableFilterExpression;

namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    /// <summary>
    /// Модель пользовательских настроек грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SettingsModel<T>
        where T : class
    {
        /// <summary>
        /// Настройки колонок
        /// </summary>
        public List<ColumnSettings<T>> Columns { get; set; }

        /// <summary>
        /// Конструктор фильтра
        /// </summary>
        public FilterExpression<T> ConstructorFilterExpression {  get; set; }

        /// <summary>
        /// Включен ли фильтр
        /// </summary>
        public bool FilterApplied { get; set; }

        /// <summary>
        /// Выбранная группировка данных
        /// </summary>
        public HashSet<string> Grouping { get; set; }
    }
}
