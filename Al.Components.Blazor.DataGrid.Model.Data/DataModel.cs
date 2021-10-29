using System.Diagnostics;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    /// <summary>
    /// Модель данных грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataModel<T>
        where T : class
    {
        /// <summary>
        /// Срабатывает, перед загрузкой данных
        /// </summary>
        public event Func<CancellationToken, Task> OnLoadDataStart;
        /// <summary>
        /// Срабатывает после окончания загрузки данных
        /// </summary>
        public event Func<long, CancellationToken, Task> OnLoadDataEnd;

        public IEnumerable<T> Data { get; private set; } = new List<T>();
        public int CountAll { get; private set; }

        readonly IDataProvider<T> _dataProvider;


        public DataModel(IDataProvider<T> dataProvider)
        {
            if(dataProvider == null)    
                throw new ArgumentNullException(nameof(dataProvider));

            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Обновляет данные
        /// </summary>
        /// <param name="cancellationToken">токен отмены асинхронной операции</param>
        /// <returns>Количество миллисекунд, затраченное на обновлене данных</returns>
        public async Task<long> RefreshData(DataPaginateRequest<T> request, CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();

            if (OnLoadDataStart != null)
                await OnLoadDataStart.Invoke(cancellationToken);

            var queryableData = await _dataProvider.LoadData(request, cancellationToken);

            stopWatch.Stop();

            CountAll = await _dataProvider.GetCount(request, cancellationToken);

            Data = await _dataProvider.GetMaterializationData(queryableData, cancellationToken);

            if (OnLoadDataEnd != null)
                await OnLoadDataEnd.Invoke(stopWatch.ElapsedMilliseconds, cancellationToken);

            return stopWatch.ElapsedMilliseconds;
        }
    }
}
