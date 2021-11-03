using Al.Components.Blazor.DataGrid.Model.Enums;

using System.ComponentModel;
using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumn<T>
        where T : class
    {
        bool Visible { get; }
        bool Sortable { get; }
        int Width { get; }
        string Title { get; }
        ListSortDirection? Sort { get; }
        bool Resizable { get; }
        ColumnFixedType FixedType { get; }
        bool Draggable { get; }
        bool Filterable { get; }
        string UniqueName { get; }
        Expression<Func<T, object>>? FieldExpression { get; }
    }
}
