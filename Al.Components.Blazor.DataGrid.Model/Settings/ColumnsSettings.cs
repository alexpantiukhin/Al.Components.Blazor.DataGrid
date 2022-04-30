using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;

namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    public class ColumnsSettings
    {
        public bool Draggable { get; set; }
        public ResizeMode ResizeMode { get; set; }
        public bool AllowResizeLastColumn { get; set; }
        public IEnumerable<ColumnSettings> Columns { get; set; }
    }
}
