using Al.Collections;
using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Components.Blazor.ResizeComponent;

using BlazorServer.Data;

using Microsoft.AspNetCore.Components;

#nullable disable

namespace BlazorServer.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public WeatherForecastService service { get; set; }

        DataGridModel model;

        ResizeArea _resizeArea;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            model = new DataGridModel(GetData, GetSettings);
            await model.ApplyDefaultSettings();
        }

        async Task<CollectionResponse> GetData(CollectionRequest response, CancellationToken cancellationToken = default)
        {
            var data = await service.GetForecastAsync(DateTime.Now);
            return new CollectionResponse
            {
                Items = data,
                TotalCount = data.Length
            };
        }

        async Task<SettingsModel> GetSettings(CancellationToken cancellationToken = default)
        {
            var columns = new ColumnSettings[]
            {
                new ColumnSettings(nameof(WeatherForecast.Date))
                {
                    Visible = true,
                    Resizable = true,
                    Title = "Дата",
                    Width = 100,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                    Sort = SortDirection.Ascending,
                },
                new ColumnSettings(nameof(WeatherForecast.TemperatureF))
                {
                    Visible=true,
                    Resizable = true,
                    Title="Температура F",
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                    Sort= SortDirection.Descending
                }
            };

            return new SettingsModel
            {
                Columns = new ColumnsSettings
                {
                    Columns = columns,
                    Draggable = true
                }
            };
        }
    }
}
