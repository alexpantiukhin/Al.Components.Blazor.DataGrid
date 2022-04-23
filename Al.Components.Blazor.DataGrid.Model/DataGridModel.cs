using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.Blazor.DataGrid.Model.Settings;

using System.Diagnostics.CodeAnalysis;
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
        public DataModel<T> Data { get; }

        /// <summary>
        /// Показывать заголовки столбцов
        /// </summary>
        public virtual bool ShowColumnsTitle { get; set; } = true;


        /// <summary>
        /// Модель без провайдера данных создавать нельзя
        /// </summary>
        DataGridModel()
        {
            throw new Exception("Вызов недопустимого конструктора");
        }



        async Task OnColumnFilterChangedHandler()
        {
            if (Filter.FilterMode != FilterMode.Row)
                return;

            await Filter.SetExpressionByColumns(Columns.All.Select(x => x.Value));
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataProvider">Провайдер данных</param>
        /// <exception cref="ArgumentNullException">Выбрасывается,если не передать провайдер данных</exception>
        public DataGridModel()
        {
            Data = new DataModel<T>();

            Columns.All.OnAddCompleted += OnAddColumnsCompletedHandler;
            Filter.OnFilterChanged += RefreshData;
        }

        public Task<long> RefreshData() =>
            Data.RefreshData(new DataPaginateRequest<T>
            {
                FilterExpression = Filter.Expression,
                Sorts = Columns.All
                    .Where(x => x.Value.Sortable && x.Value.Sort != null)
                    .OrderBy(x => x.Index)
                    .ToDictionary(x => x.Value.UniqueName, x => x.Value.Sort.Value),
                Skip = Paginator.Page == 0 ? 0 : Paginator.Step == 1 ? Paginator.Step : (Paginator.Step - 1),
                Take = Paginator.Step
            });


        public async Task<Result> ApplySettings(string jsonString)
        {
            Result result = new();
            SettingsModel<T> settings;

            try
            {
                settings = JsonSerializer.Deserialize<SettingsModel<T>>(jsonString)
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

            Filter.ApplySettings(settings.ConstructorFilterExpression, settings.FilterApplied);

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


        public event Func<SettingsModel<T>, Task> OnSettingsChanged;
    }
}
