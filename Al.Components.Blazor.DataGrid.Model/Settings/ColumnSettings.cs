using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;

namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    public class ColumnSettings : IColumn
    {
        public string? UniqueName { get; set; }
        public bool Visible { get; set; }
        public SortDirection? Sort { get; set; }
        public int Width { get; set; }
        public ColumnFixedType FixedType { get; set; }
        public RequestFilter? Filter { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? HeaderComponentTypeName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? CellComponentTypeName { get; set; }
        public bool Sortable { get; set; }
        public string? Title { get; set; }
        public bool Resizable { get; set; }
        public bool Filterable { get; set; }
    }
}
