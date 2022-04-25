namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    /// <summary>
    /// Модель пользовательских настроек грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SettingsModel
    {
        /// <summary>
        /// Настройки колонок
        /// </summary>
        public ColumnsSettings Columns { get; set; }

        /// <summary>
        /// Конструктор фильтра
        /// </summary>
        public RequestFilter? ConstructorFilter {  get; set; }

        /// <summary>
        /// Включен ли фильтр
        /// </summary>
        public bool FilterApplied { get; set; }

        /// <summary>
        /// Выбранная группировка данных
        /// </summary>
        public HashSet<string>? Grouping { get; set; }
    }
}
