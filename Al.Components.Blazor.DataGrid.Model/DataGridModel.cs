using Al.Components.Blazor.DataGrid.Model.Interfaces;

using System.Diagnostics.CodeAnalysis;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель грида
    /// </summary>
    /// <typeparam name="T">Тип записи грида</typeparam>
    public class DataGridModel<T>
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
        /// <summary>
        /// Показывать строку фильтров
        /// </summary>
        public FilterMode FilterMode { get; set; }

        /// <summary>
        /// Модель столбцов
        /// </summary>
        public ColumnsModel<T> Columns { get; private set; }
        /// <summary>
        /// Задержка лоадера, мс
        /// </summary>
        public int LoaderDelay { get; set; } = 100;
        /// <summary>
        /// Модель данных
        /// </summary>
        public DataModel<T> Data { get; }

        /// <summary>
        /// Модель без провайдера данных создавать нельзя
        /// </summary>
        DataGridModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataProvider">Провайдер данных</param>
        /// <exception cref="ArgumentNullException">Выбрасывается,если не передать провайдер данных</exception>
        public DataGridModel([NotNull] IDataProvider<T> dataProvider)
        {
            if(dataProvider == null)    
                throw new ArgumentNullException(nameof(dataProvider));

            Data = new DataModel<T>(dataProvider, this);
        }



    }
}
