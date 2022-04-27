using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Helpers.Throws;

using Microsoft.Extensions.Logging;

using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель грида
    /// </summary>
    public class DataGridModel
    {
        public ILogger Logger { get; init; }
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
        /// Модель столбцов.
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

        readonly Func<CancellationToken, Task<SettingsModel>> _getSettingsFuncAsync;

        /// <summary>
        /// Конструктор из метода получения данных
        /// </summary>
        public DataGridModel(
            Func<CollectionRequest, CancellationToken, Task<CollectionResponse>> getDataFuncAsync,
            Func<CancellationToken, Task<SettingsModel>> getSettingsFuncAsync)
        {
            ParametersThrows.ThrowIsNull(getDataFuncAsync, nameof(getDataFuncAsync));
            ParametersThrows.ThrowIsNull(getSettingsFuncAsync, nameof(getSettingsFuncAsync));

            _getSettingsFuncAsync = getSettingsFuncAsync;
            Data = new(getDataFuncAsync, Columns, Filter, Paginator);
        }

        //public Task<long> RefreshData(CancellationToken cancellationToken) =>
        //    Data.Refresh();

        //new DataPaginateRequest<T>
        //    {
        //        FilterExpression = Filter.Expression,
        //        Sorts = Columns.All
        //            .Where(x => x.Value.Sortable && x.Value.Sort != null)
        //            .OrderBy(x => x.Index)
        //            .ToDictionary(x => x.Value.UniqueName, x => x.Value.Sort.Value),
        //        Skip = Paginator.Page == 0 ? 0 : Paginator.PageSize == 1 ? Paginator.PageSize : (Paginator.PageSize - 1),
        //        Take = Paginator.PageSize
        //    });

        public async Task<Result> ResetToDefaultSettings(CancellationToken cancellationToken = default)
        {
            Result result = new ();

            var setting = await _getSettingsFuncAsync(cancellationToken);

            if (setting == null)
                return result.AddError("Не удалось получить настройки");

            return result;
        }

        public async Task<Result> ApplyDefaultSettings(CancellationToken cancellationToken = default)
        {
            var settings = await _getSettingsFuncAsync(cancellationToken);

            Result result = new();

            if (settings.Columns != null)
            {
                var columnsResult = await Columns.ApplySettings(settings.Columns, cancellationToken);

                if (!columnsResult.Success)
                    return columnsResult;
            }

            if (OnSettingsChanged != null)
                await OnSettingsChanged.Invoke(settings, cancellationToken);

            Filter.ApplySettings(settings.ConstructorFilter, settings.FilterApplied, cancellationToken);

            // todo обработать группировку

            await Data.Refresh(cancellationToken);

            return result;
        }

#nullable disable
        public event Func<SettingsModel, CancellationToken, Task> OnSettingsChanged;
    }
}
