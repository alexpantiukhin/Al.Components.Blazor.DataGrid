using Al.Collections.QueryableFilterExpression;

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
        public event Func<CancellationToken, Task>? OnLoadDataStart;
        /// <summary>
        /// Срабатывает после окончания загрузки данных
        /// </summary>
        public event Func<long, CancellationToken, Task>? OnLoadDataEnd;

        public IEnumerable<T> Data { get; private set; } = new List<T>();
        public int CountAll { get; private set; }

        readonly IDataProvider<T> _dataProvider;
        readonly IOperationExpressionResolver _operationExpressionResolver;

        /// <summary>
        /// Нельзя использовать конструктор без параметров
        /// </summary>
        DataModel() { throw new Exception("Вызов недопустимого конструктора"); }

        public DataModel(IDataProvider<T> dataProvider, IOperationExpressionResolver operationExpressionResolver)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _operationExpressionResolver = operationExpressionResolver ?? throw new ArgumentNullException(nameof(operationExpressionResolver));
        }

        /// <summary>
        /// Обновляет данные
        /// </summary>
        /// <param name="cancellationToken">токен отмены асинхронной операции</param>
        /// <returns>Количество миллисекунд, затраченное на обновлене данных</returns>
        public async Task<long> RefreshData(DataPaginateRequest<T> request, CancellationToken cancellationToken = default)
        {
            if(request is null)
                throw new ArgumentNullException(nameof(request));   

            var stopWatch = new Stopwatch();

            if (OnLoadDataStart != null)
                await OnLoadDataStart.Invoke(cancellationToken);

            var allQuery = await _dataProvider.LoadData(cancellationToken);

            var paginationQuery = request.Apply(allQuery, _operationExpressionResolver);

            stopWatch.Start();
            Data = await _dataProvider.GetMaterializationData(paginationQuery, cancellationToken);
            stopWatch.Stop();

            CountAll = await _dataProvider.GetCount(allQuery, cancellationToken);

            if (OnLoadDataEnd != null)
                await OnLoadDataEnd.Invoke(stopWatch.ElapsedMilliseconds, cancellationToken);

            return stopWatch.ElapsedMilliseconds;
        }
    }
}
