using Al.Components.Blazor.DataGrid.Model.Enums;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumnsCommon
    {
        bool Draggable { get; }
        ResizeMode ResizeMode { get; }
        bool AllowResizeLastColumn { get; }
        bool AllowFrozenLeftChanging { get; }
        bool AllowFrozenRightChanging { get; }
    }
}
