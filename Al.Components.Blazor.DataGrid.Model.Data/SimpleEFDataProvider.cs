using Microsoft.EntityFrameworkCore;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    /// <summary>
    /// Провайдер данных для EnitytFrameworkCore
    /// </summary>
    /// <typeparam name="T">Тип записи</typeparam>
    public class SimpleEFDataProvider<T> : IDataProvider<T>
        where T : class
    {
        readonly IQueryable<T> _data;
        public SimpleEFDataProvider(IQueryable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _data = source;
        }

        public Task<int> GetCount(IQueryable<T> data, CancellationToken token) =>
            data.CountAsync(token);

        public Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token) =>
            data.ToArrayAsync(token);

        public Task<IQueryable<T>> LoadData(CancellationToken token) =>
            Task.FromResult(_data);
    }
}
