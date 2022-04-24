namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumns
    {
        Task SortChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task FixedTypeChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task VisibleChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
        Task FilterChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default);
    }
}
