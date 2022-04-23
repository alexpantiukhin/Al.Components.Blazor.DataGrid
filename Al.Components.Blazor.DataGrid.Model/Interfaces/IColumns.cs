namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumns
    {
        Task SortChangedNotify(ColumnModel columnModel);
        Task FixedTypeChangedNotify(ColumnModel columnModel);
        Task VisibleChangedNotify(ColumnModel columnModel);
        Task FilterChangedNotify(ColumnModel columnModel);
    }
}
