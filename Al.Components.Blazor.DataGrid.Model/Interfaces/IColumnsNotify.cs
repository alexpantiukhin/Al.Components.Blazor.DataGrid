using Al.Collections.Orderable;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumnsNotify
    {
        Task SortChangedNotify(OrderableDictionaryNode<string, ColumnModel> columnModel, CancellationToken cancellationToken = default);
        Task SortIndexChangedNotify(OrderableDictionaryNode<string, ColumnModel> columnModel, CancellationToken cancellationToken = default);
        Task FrozenTypeChangedNotify(OrderableDictionaryNode<string, ColumnModel> columnModel, CancellationToken cancellationToken = default);
        Task VisibleChangedNotify(OrderableDictionaryNode<string, ColumnModel> columnModel, CancellationToken cancellationToken = default);
        Task FilterChangedNotify(OrderableDictionaryNode<string, ColumnModel> columnModel, CancellationToken cancellationToken = default);
    }
}
