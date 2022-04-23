﻿using Al.Collections;
using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Helpers.Throws;

using System.Collections;
using System.Diagnostics;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    /// <summary>
    /// Модель данных грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataModel
    {
        public IEnumerable? Data { get; private set; }
        public int TotalCount { get; private set; }

        readonly IEnumerable? _items;
        readonly Func<CollectionRequest, CancellationToken, Task<CollectionResponse>>? _getDataAsync;
        private readonly ColumnsModel _columnsModel;
        private readonly FilterModel _filterModel;
        private readonly PaginatorModel _paginatorModel;

        DataModel(ColumnsModel columnsModel,
            FilterModel filterModel,
            PaginatorModel paginatorModel)
        {
            ParametersThrows.ThrowIsNull(columnsModel, nameof(columnsModel));
            ParametersThrows.ThrowIsNull(filterModel, nameof(filterModel)); 
            ParametersThrows.ThrowIsNull(paginatorModel, nameof(paginatorModel));

            _columnsModel = columnsModel;
            _filterModel = filterModel;
            _paginatorModel = paginatorModel;
        }

        /// <summary>
        /// Конструкто из материализованных данных
        /// </summary>
        public DataModel(
            IEnumerable items,
            ColumnsModel columnsModel,
            FilterModel filterModel,
            PaginatorModel paginatorModel) : this(columnsModel, filterModel, paginatorModel)
        {
            ParametersThrows.ThrowIsNull(_items, nameof(_items));

            _items = items;
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
                Data = response.Items;
                TotalCount = response.TotalCount;
            }

            if(_items != null)
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
                Sorts = _columnsModel.All
                    .Where(x => x.Value.Sortable && x.Value.Sort != null)
                    .OrderBy(x => x.Index)
                    .ToDictionary(x => x.Value.UniqueName, x => x.Value.Sort.Value)
            };
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
