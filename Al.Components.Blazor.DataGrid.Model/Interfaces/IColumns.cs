using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model.Enums;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumns
    {
        bool Draggable { get; }
        ResizeMode ResizeMode { get; }
        bool AllowResizeLastColumn { get; }
        OrderableDictionaryNode<string, ColumnModel>[] Visibilities { get; }


        event Func<ColumnModel, CancellationToken, Task>? OnResizeEnd;
        event Func<CancellationToken, Task>? OnDraggableChanged;

    }
}
