using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Al.Components.Blazor.AlDataGrid.Model
{
    public class DataModel<T>
        where T : class
    {
        internal event Func<Task> OnLoadDataStart;
        internal event Func<Task> OnLoadDataEnd;
        internal event Func<ColumnModel<T>, Task> OnSorted;

        public IEnumerable<T> Data { get; private set; }
        public int CountAll { get; private set; }

        int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 1)
                    _pageSize = 1;
                else
                    _pageSize = value;

            }
        }
        public int PageIndex { get; set; }


        IQueryable<T> FilterationData;
        IQueryable<T> PaginationData;
        readonly IDataProvider<T> _dataProvider;
        readonly DataGridModel<T> _gridModel;

        internal DataModel(IDataProvider<T> dataProvider, DataGridModel<T> gridModel)
        {
            _dataProvider = dataProvider;
            _gridModel = gridModel;
        }

        internal async Task SortHandler(ColumnModel<T> column)
        {
            if (!column.Sortable)
                return;

            if (column.Sort == EnumSort.None)
                column.Sort = EnumSort.Ascending;
            else if (column.Sort == EnumSort.Ascending)
                column.Sort = EnumSort.Descending;
            else
                column.Sort = EnumSort.None;

            await RefreshData();

            if (OnSorted != null)
                await OnSorted(column);
        }


        /// <summary>
        /// Обновляет данные
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="showPaginator"></param>
        /// <param name="token"></param>
        /// <returns>Время, затраченное на обновлене данных</returns>
        internal async Task<TimeSpan> RefreshData()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var dateStart = DateTime.Now;

            if (OnLoadDataStart != null)
                await OnLoadDataStart.Invoke();

            await RefreshFilterAndSortQueryable(cts.Token);

            await RefreshPaginationData(cts.Token);

            if (OnLoadDataEnd != null)
                await OnLoadDataEnd.Invoke();

            return DateTime.Now - dateStart;
        }
        async Task RefreshFilterAndSortQueryable(CancellationToken token)
        {
            FilterationData = await _dataProvider.LoadData(token);

            foreach (var column in _gridModel.Columns.All)
            {
                if (column.FilterExpression != null)
                    FilterationData = FilterationData.Where(column.FilterExpression);
            }

            bool hasSort = false;
            foreach (var column in _gridModel.Columns.All)
            {
                if (column.Sortable && column.Sort != EnumSort.None)
                {
                    FilterationData = column.AddSort(FilterationData, hasSort);

                    hasSort = true;
                }
            }
        }

        void RefreshPaginationQueryable(bool showPaginator)
        {
            if (showPaginator)
                PaginationData = FilterationData.Skip(PageSize * PageIndex).Take(PageSize);
            else
                PaginationData = FilterationData;
        }


        async Task RefreshPaginationData(CancellationToken token)
        {
            //Loader.Show();
            RefreshPaginationQueryable(_gridModel.ShowPaginator);


            CountAll = await _dataProvider.GetCount(FilterationData, token);
            Data = await _dataProvider.GetMaterializationData(PaginationData, token);

            //await Paginator.AllCountChange(AllRowsCount);
            //await Body.RowsChange(_rows);

            //jSInteropExtension.ConsoleLog($"Time request and rendering data: {(timeEnd - timeStart).TotalMilliseconds}ms");

            //Loader.Hide();
        }

    }
}
