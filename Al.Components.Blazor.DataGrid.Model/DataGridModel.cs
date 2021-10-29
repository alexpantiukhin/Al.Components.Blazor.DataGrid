using Al.Components.Blazor.DataGrid.Model.Data;

using System.Diagnostics.CodeAnalysis;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель грида
    /// </summary>
    /// <typeparam name="T">Тип записи грида</typeparam>
    public class DataGridModel<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Модель строк
        /// </summary>
        public RowsModel Rows { get; } = new();
        /// <summary>
        /// Модель пагинатора
        /// </summary>
        public PaginatorModel Paginator { get; } = new();

        public FilterModel<T> Filter { get; } = new();

        /// <summary>
        /// Модель столбцов
        /// </summary>
        public ColumnsModel<T> Columns { get; private set; } = new();
        /// <summary>
        /// Задержка лоадера, мс
        /// </summary>
        public int LoaderDelay { get; set; } = 100;
        /// <summary>
        /// Модель данных
        /// </summary>
        public DataModel<T> Data { get; }

        /// <summary>
        /// Показывать заголовки столбцов
        /// </summary>
        public virtual bool ShowTitleColumns { get; set; } = true;


        /// <summary>
        /// Модель без провайдера данных создавать нельзя
        /// </summary>
        DataGridModel()
        {
            Filter.OnFilterChange += RefreshData;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataProvider">Провайдер данных</param>
        /// <exception cref="ArgumentNullException">Выбрасывается,если не передать провайдер данных</exception>
        public DataGridModel([NotNull] IDataProvider<T> dataProvider) : this()
        {
            if (dataProvider == null)
                throw new ArgumentNullException(nameof(dataProvider));

            Data = new DataModel<T>(dataProvider);
        }

        public Task<long> RefreshData() =>
            Data.RefreshData(new DataPaginateRequest<T>
            {
                FilterExpression = Filter.Expression,
                Sorts = Columns.All
                    .Where(x => x.Value.Sortable && x.Value.Sort != null)
                    .ToDictionary(x => x.Value.UniqueName, x => x.Value.Sort.Value)
            });

        public void Dispose()
        {
            Filter.OnFilterChange -= RefreshData;
        }
    }
}
