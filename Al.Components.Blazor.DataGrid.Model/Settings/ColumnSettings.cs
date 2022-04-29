using Al.Collections;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Helpers.Throws;

namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    public class ColumnSettings : IColumn
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string UniqueName { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SortDirection? Sort { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int SortIndex { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ColumnFrozenType FrozenType { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public RequestFilter? Filter { get; set; }
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
        public bool Sortable { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Resizable { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Filterable { get; set; }

        /// <summary>
        /// Обязательно уникальное имя столбца
        /// </summary>
        /// <param name="uniqueName">Имя столбца</param>
        public ColumnSettings(string uniqueName)
        {
            ParametersThrows.ThrowIsWhitespace(uniqueName, nameof(uniqueName));
            UniqueName = uniqueName;
        }
    }
}
