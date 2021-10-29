using System.ComponentModel;
using System.Diagnostics;
using Al.Collections.ExpressionExtensions;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    /// <summary>
    /// Модель данных грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataModel<T>
        where T : class
    {
        internal event Func<CancellationToken, Task> OnLoadDataStart;
        internal event Func<CancellationToken, Task> OnLoadDataEnd;

        public IEnumerable<T> Data { get; private set; }
        public int CountAll { get; private set; }

        IQueryable<T> QueryableData;

        readonly IDataProvider<T> _dataProvider;

        public DataModel(IDataProvider<T> dataProvider)
        {
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

            QueryableData = await _dataProvider.LoadData(request, cancellationToken);

            stopWatch.Stop();

            CountAll = await _dataProvider.GetCount(request, cancellationToken);

            Data = await _dataProvider.GetMaterializationData(QueryableData, cancellationToken);

            if (OnLoadDataEnd != null)
                await OnLoadDataEnd.Invoke(cancellationToken);

            return stopWatch.ElapsedMilliseconds;
        }

        //async Task RefreshFilterAndSortQueryable(CancellationToken token)
        //{
        //    var filterExpression = _filterExpressionResolver();

        //    FilterationData = await _dataProvider.LoadData(filterExpression, token);

        //    var sortResolve = _sortResolver();

        //    foreach (var item in sortResolve)
        //    {
        //        if (item.Value == SortType.None)
        //            continue;

        //        if(item.Value == SortType.Ascending)
        //        {
        //            if (FilterationData is IOrderedQueryable<T> orderedQueryable)
        //                FilterationData = orderedQueryable.ThenBy(item.Key);
        //            else
        //                FilterationData = FilterationData.OrderBy(item.Key);
        //        }
        //        else
        //        {
        //            if (FilterationData is IOrderedQueryable<T> orderedQueryable)
        //            {
        //                FilterationData = orderedQueryable.ThenByDescending(item.Key);
        //            }
        //            else
        //                FilterationData = FilterationData.OrderByDescending(item.Key);
        //        }
        //    }
        //}

        //void RefreshPaginationQueryable(bool showPaginator)
        //{
        //    if (showPaginator)
        //        PaginationData = FilterationData.Skip(PageSize * PageIndex).Take(PageSize);
        //    else
        //        PaginationData = FilterationData;
        //}


        //async Task RefreshPaginationData(CancellationToken token)
        //{
        //    //Loader.Show();
        //    RefreshPaginationQueryable(_gridModel.Paginator.Show);


        //    //CountAll = await _dataProvider.GetCount(FilterationData, token);
        //    Data = await _dataProvider.GetMaterializationData(PaginationData, token);

        //    //await Paginator.AllCountChange(AllRowsCount);
        //    //await Body.RowsChange(_rows);

        //    //jSInteropExtension.ConsoleLog($"Time request and rendering data: {(timeEnd - timeStart).TotalMilliseconds}ms");

        //    //Loader.Hide();
        //}


        //public override string ToString()
        //{
        //    var sb = new StringBuilder();
        //    foreach (var item in Data)
        //    {
        //        sb.Append('|');
        //        for (int i = 0; i < _gridModel.Columns.Visibilities.Length; i++)
        //        {
        //            var column = _gridModel.Columns.Visibilities[i];
        //            column.GetColumnValue(item);
        //            sb.Append('|');

        //            if (i < _gridModel.Columns.Visibilities.Length - 1)
        //                sb.Append('\t');
        //        }
        //        sb.AppendLine();
        //    }

        //    return sb.ToString();
        //}
    }
}
