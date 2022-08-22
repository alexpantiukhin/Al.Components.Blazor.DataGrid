using Al.Components.Blazor.DataGrid.Model.Enums;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IRows
    {
        SelectableRowMode SelectableRowMode { get; }
        bool ShowDetail { get; }
    }
}
