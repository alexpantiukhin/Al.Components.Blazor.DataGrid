using System;
using System.Collections.Generic;
using System.Linq;

namespace Al.Components.Blazor.AlDataGrid.Model
{
    /// <summary>
    /// Модель грида
    /// </summary>
    /// <typeparam name="T">Тип записи грида</typeparam>
    internal class DataGridModel<T>
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
        public bool ShowFilterRow { get; set; }
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
        /// Модель без провайдера данных создать невозможно
        /// </summary>
        DataGridModel() { }

        public DataGridModel(IDataProvider<T> dataProvider)
        {
            Data = new DataModel<T>(dataProvider, this);
        }

        readonly List<ColumnModel<T>> TemplateColumnsList = new();

        /// <summary>
        /// Добавить столбец к набору
        /// </summary>
        /// <param name="column"></param>
        public void Add(ColumnModel<T> column)
        {
            if (column == null) return;

            if (TemplateColumnsList.Any(x => x.UniqueName == column.UniqueName))
                throw new ArgumentException("The column with the specified name is already in the list");

            TemplateColumnsList.Add(column);
        }


        /// <summary>
        /// Завершить формирование столбцов.<br/>
        /// Запускается сразу после инициализации компонента.
        /// </summary>
        public void FinishCreateColumns()
        {
            Columns = new(TemplateColumnsList);
            TemplateColumnsList.Clear();
        }

    }
}
