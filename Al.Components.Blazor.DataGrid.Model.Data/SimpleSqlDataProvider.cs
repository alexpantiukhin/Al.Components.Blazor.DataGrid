using Microsoft.EntityFrameworkCore;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class SimpleSqlDataProvider<T> : IDataProvider<T>
        where T : class
    {
        readonly IQueryable<T> _data;
        public SimpleSqlDataProvider(IQueryable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _data = source;
        }
        public Task<int> GetCount(DataRequest<T> request, CancellationToken token) =>
            request.Apply(_data).CountAsync(token);

        public Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token) =>
            data.ToArrayAsync(token);

        public Task<IQueryable<T>> LoadData(DataPaginateRequest<T> request, CancellationToken token) =>
            Task.FromResult(request.Apply(_data));
    }
}
