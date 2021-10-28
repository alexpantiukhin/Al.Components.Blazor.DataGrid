using Al.Components.QueryableFilterExpression;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public interface IDataProvider<T>
        where T: class
    {
        Task<IQueryable<T>> LoadData(DataPaginateRequest<T> request, CancellationToken token);

        Task<int> GetCount(DataRequest<T> request, CancellationToken token);

        Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token);
    }
}
