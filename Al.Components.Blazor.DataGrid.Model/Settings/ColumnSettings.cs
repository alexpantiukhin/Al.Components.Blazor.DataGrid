using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;

using System.ComponentModel;

namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    public class ColumnSettings<T>
        where T : class
    {
        public string UniqueName { get; set; }
        public bool Visible { get; set; }
        public ListSortDirection? Sort { get; set; }
        public int Width { get; set; }
        public ColumnFixedType FixedType { get; set; }
        public FilterExpression<T> FilterExpression { get; set; }
    }
}
