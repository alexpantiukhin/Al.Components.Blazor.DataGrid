using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumnsSettings : IColumnsCommon
    {
        IEnumerable<ColumnSettings> Columns { get; }

    }
}
