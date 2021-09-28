namespace Al.Components.Blazor.AlDataGrid.Model
{
    /// <summary>
    /// Модель строк грида
    /// </summary>
    internal class RowsModel
    {
        /// <summary>
        /// Показывать детализацию
        /// </summary>
        public bool ShowDetail { get; set; }
        /// <summary>
        /// Генерирует событие клика по строке
        /// </summary>
        public bool Clickable { get; set; }
        /// <summary>
        /// Возможность выбирать строки
        /// </summary>
        public bool Selectable { get; set; }
        /// <summary>
        /// Режим выбора строк
        /// </summary>
        public EnumSelectableRowMode SelectableRowMode { get; set; } = EnumSelectableRowMode.Once;

    }
}
