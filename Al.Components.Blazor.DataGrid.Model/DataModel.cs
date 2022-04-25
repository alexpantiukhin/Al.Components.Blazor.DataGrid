using Al.Collections.Api;
using Al.Helpers.Throws;

using System.Collections;
using System.Diagnostics;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    /// <summary>
    /// Модель данных грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataModel : IDisposable
    {
        public IEnumerable? Items { get; set; }
        public int TotalCount { get; private set; }

        readonly Func<CollectionRequest, CancellationToken, Task<CollectionResponse>>? _getDataAsync;
        private readonly ColumnsModel _columnsModel;
        private readonly FilterModel _filterModel;
        private readonly PaginatorModel _paginatorModel;

        public DataModel(ColumnsModel columnsModel,
            FilterModel filterModel,
            PaginatorModel paginatorModel)
        {
            ParametersThrows.ThrowIsNull(columnsModel, nameof(columnsModel));
            ParametersThrows.ThrowIsNull(filterModel, nameof(filterModel)); 
            ParametersThrows.ThrowIsNull(paginatorModel, nameof(paginatorModel));

            _columnsModel = columnsModel;
            _filterModel = filterModel;
            _paginatorModel = paginatorModel;

            _columnsModel.OnFilterColumnChanged += OnColumnFilterChangedHandler;
            _columnsModel.OnSortColumnChanged += OnColumnSortChangedHandler;
        }

        /// <summary>
        /// Конструкто из метода, получающего данные
        /// </summary>
        public DataModel(
            Func<CollectionRequest, CancellationToken, Task<CollectionResponse>> getDataFuncAsync,
            ColumnsModel columnsModel,
            FilterModel filterModel,
            PaginatorModel paginatorModel) : this(columnsModel, filterModel, paginatorModel)
        {
            ParametersThrows.ThrowIsNull(getDataFuncAsync, nameof(getDataFuncAsync));
            _getDataAsync = getDataFuncAsync;
        }

        /// <summary>
        /// Обновляет данные
        /// </summary>
        /// <param name="cancellationToken">токен отмены асинхронной операции</param>
        /// <returns>Количество миллисекунд, затраченное на обновлене данных</returns>
        public async Task<long> Refresh(CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();

            if (OnLoadDataStart != null)
                await OnLoadDataStart.Invoke(cancellationToken);

            if(_getDataAsync != null)
            {
                var response = await _getDataAsync(PrepareRequest(), cancellationToken);
                Items = response.Items;
                TotalCount = response.TotalCount;
            }
            else
            {

            }

            if (OnLoadDataEnd != null)
                await OnLoadDataEnd.Invoke(stopWatch.ElapsedMilliseconds, cancellationToken);

            return stopWatch.ElapsedMilliseconds;
        }

        CollectionRequest PrepareRequest()
        {
            return new CollectionRequest
            {
                Filter = _filterModel.RequestFilter,
                Sorts = _columnsModel.Sorts
                    .ToDictionary(x => x.UniqueName, x => x.Sort.Value),
                Page = _paginatorModel.Page,
                PageSize = _paginatorModel.PageSize,
            };
        }

        async Task OnColumnFilterChangedHandler(ColumnModel column, CancellationToken cancellationToken = default)
        {
            await _filterModel.SetExpressionByColumns(_columnsModel.All.Select(x => x), cancellationToken);
            await Refresh(cancellationToken);
        }

        Task OnColumnSortChangedHandler(ColumnModel column, CancellationToken cancellationToken = default)
        {
            return Refresh(cancellationToken);
        }

        public void Dispose()
        {
            _columnsModel.OnFilterColumnChanged -= OnColumnFilterChangedHandler;
            _columnsModel.OnSortColumnChanged -= OnColumnSortChangedHandler;
        }

#nullable disable
        /// <summary>
        /// Срабатывает, перед загрузкой данных
        /// </summary>
        public event LoadDataStartDelegate OnLoadDataStart;
        /// <summary>
        /// Срабатывает после окончания загрузки данных
        /// </summary>
        public event LoadDataEndDelegate OnLoadDataEnd;
    }

    public delegate Task LoadDataEndDelegate(long elapsedMilliseconds, CancellationToken cancellationToken = default);
    public delegate Task LoadDataStartDelegate(CancellationToken cancellationToken = default);
}
