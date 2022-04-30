using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Settings;

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
                },
                new ColumnSettings(nameof(WeatherForecast.TemperatureF))
                {
                    Visible=true,
                    Resizable = true,
                    Title="Температура F",
                    Width = 200
                }
            };

            return new SettingsModel
            {
                Columns = new ColumnsSettings
                {
                    Columns = columns
                }
            };
        }
    }
}
