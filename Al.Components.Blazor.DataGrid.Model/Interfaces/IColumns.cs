using Al.Collections.Orderable;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumns : IColumnsCommon
    {
        OrderableDictionaryNode<string, ColumnModel>[] Visibilities { get; }
        Task SortChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task SortIndexChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task FrozenTypeChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task VisibleChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task FilterChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);


        event Func<ColumnModel, CancellationToken, Task>? OnResizeEnd;
        event Func<CancellationToken, Task>? OnDraggableChanged;

    }
}
