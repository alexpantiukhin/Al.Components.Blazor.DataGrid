using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.Blazor.DataGrid.Model.Settings;

using System.Collections;
using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель грида
    /// </summary>
    /// <typeparam name="T">Тип записи грида</typeparam>
    public class DataGridModel : IDisposable
    {
        /// <summary>
        /// Модель строк
        /// </summary>
        public RowsModel Rows { get; } = new();
        /// <summary>
        /// Модель пагинатора
        /// </summary>
        public PaginatorModel Paginator { get; } = new();

        public FilterModel Filter { get; } = new();

        /// <summary>
        /// Модель столбцов
        /// </summary>
        public ColumnsModel Columns { get; private set; } = new();
        /// <summary>
        /// Задержка лоадера, мс
        /// </summary>
        public int LoaderDelay { get; set; } = 100;
        /// <summary>
        /// Модель данных
        /// </summary>
        public DataModel Data { get; }

        /// <summary>
        /// Показывать заголовки столбцов
        /// </summary>
        public virtual bool ShowColumnsTitle { get; set; } = true;


        async Task OnColumnFilterChangedHandler()
        {
            if (Filter.FilterMode != FilterMode.Row)
                return;

            await Filter.SetExpressionByColumns(Columns.All.Select(x => x.Value));
        }


        public DataGridModel()
        {
            Columns.All.OnAddCompleted += OnAddColumnsCompletedHandler;
            Filter.OnFilterChanged += RefreshData;
        }

        /// <summary>
        /// Конструктор из набора данных
        /// </summary>
        public DataGridModel(IEnumerable items) : this()
        {
            Data = new(items);
        }

        /// <summary>
        /// Конструктор из метода получения данных
        /// </summary>
        public DataGridModel(Func<CollectionRequest, CancellationToken, Task<CollectionResponse>> getDataFuncAsync) : this()
        {
            Data = new(getDataFuncAsync);
        }

        public Task<long> RefreshData(CancellationToken cancellationToken) =>
            Data.Refresh(new );
        
        new DataPaginateRequest<T>
            {
                FilterExpression = Filter.Expression,
                Sorts = Columns.All
                    .Where(x => x.Value.Sortable && x.Value.Sort != null)
                    .OrderBy(x => x.Index)
                    .ToDictionary(x => x.Value.UniqueName, x => x.Value.Sort.Value),
                Skip = Paginator.Page == 0 ? 0 : Paginator.PageSize == 1 ? Paginator.PageSize : (Paginator.PageSize - 1),
                Take = Paginator.PageSize
            });


        public async Task<Result> ApplySettings(string jsonString)
        {
            Result result = new();
            SettingsModel settings;

            try
            {
                settings = JsonSerializer.Deserialize<SettingsModel>(jsonString)
                    ?? throw new ArgumentException("Ошибочная строка настроек", nameof(jsonString));
            }
            catch (Exception ex)
            {
                return result.AddError(ex, "Ошибочная строка настроек");
            }

            if (settings == null)
                return result.AddError("Не удалось считать настройки");

            if(settings.Columns != null)
            {
                var columnsResult = await Columns.ApplySettings(settings.Columns);

                if (!columnsResult.Success)
                    return columnsResult;
            }


            if (OnSettingsChanged != null)
                await OnSettingsChanged.Invoke(settings);

            Filter.ApplySettings(settings.ConstructorFilter, settings.FilterApplied);

            // todo обработать группировку

            await RefreshData();

            return result;
        }

        void OnAddColumnsCompletedHandler()
        {
            foreach (var node in Columns.All)
            {
                node.Value.OnSortChanged += RefreshData;
                node.Value.OnFilterChanged += OnColumnFilterChangedHandler;
            }
        }
        public void Dispose()
        {
            foreach (var node in Columns.All)
            {
                node.Value.OnSortChanged -= RefreshData;
                node.Value.OnFilterChanged -= OnColumnFilterChangedHandler;
            }
            Columns.All.OnAddCompleted -= OnAddColumnsCompletedHandler;

            Filter.OnFilterChanged -= RefreshData;
        }


        public event Func<SettingsModel, Task> OnSettingsChanged;
    }
}
