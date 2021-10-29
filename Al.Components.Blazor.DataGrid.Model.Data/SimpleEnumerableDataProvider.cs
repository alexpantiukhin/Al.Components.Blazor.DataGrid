namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class SimpleEnumerableDataProvider<T> : IDataProvider<T>
        where T : class
    {
        readonly IEnumerable<T> _data;
        public SimpleEnumerableDataProvider(IEnumerable<T> data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            _data = data;
        }
        public Task<int> GetCount(DataRequest<T> request, CancellationToken token) =>
            Task.FromResult(request.Apply(_data.AsQueryable()).Count());

        public Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token) =>
            Task.FromResult(data.ToArray());

        public Task<IQueryable<T>> LoadData(DataPaginateRequest<T> request, CancellationToken token) =>
            Task.FromResult(request.Apply(_data.AsQueryable()));
    }
}
