using Al.Components.QueryableFilterExpression;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IDataProvider<T>
        where T: class
    {
        Task<IQueryable<T>> LoadData(FilterExpression<T> filterExpression, CancellationToken token);

        Task<int> GetCount(FilterExpression<T> filterExpression, CancellationToken token);

        Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token);
    }
}
