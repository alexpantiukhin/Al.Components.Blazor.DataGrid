namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class SimpleListDataProvider<T> : IDataProvider<T>
        where T : class
    {
        readonly List<T> _data;
        public SimpleListDataProvider(List<T> data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            _data = data;
        }
        public Task<int> GetCount(IQueryable<T> data, CancellationToken token) =>
            Task.FromResult(data.Count());

        public Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token) =>
            Task.FromResult(data.ToArray());

        public Task<IQueryable<T>> LoadData(CancellationToken token) =>
            Task.FromResult(_data.AsQueryable());
    }
}
