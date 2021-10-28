using Al.Components.Blazor.DataGrid.Model.Enums;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель строк грида
    /// </summary>
    public class RowsModel
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
        public SelectableRowMode SelectableRowMode { get; set; } = SelectableRowMode.Once;

    }
}
