namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель пагинатора грида
    /// </summary>
    public class PaginatorModel
    {
        /// <summary>
        /// Шаг пагинации
        /// </summary>
        public IEnumerable<int> Steps { get; set; } = new int[] { 10, 20, 50, 100 };
        /// <summary>
        /// Показывать пагинатор
        /// </summary>
        public bool Show { get; set; }
    }
}
