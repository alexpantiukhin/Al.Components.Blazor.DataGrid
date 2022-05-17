using Al.Collections;
using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Components.Blazor.ResizeComponent;


using Common.Data;

using Microsoft.AspNetCore.Components;

#nullable disable

namespace Common
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
                },
                new ColumnSettings(nameof(WeatherForecast.Summary))
                {
                    Visible=true,
                    Resizable = true,
                    Title="Summary",
                    Width = 200,
                    FrozenType = ColumnFrozenType.Right,
                    Sortable = true,
                    Sort= SortDirection.Descending
                },
                new ColumnSettings(nameof(WeatherForecast.TemperatureC))
                {
                    Visible=true,
                    Resizable = true,
                    Title="Температура C",
                    Width = 200,
                    FrozenType = ColumnFrozenType.Left,
                    Sortable = true,
                    Sort= SortDirection.Descending
                },
                new ColumnSettings(nameof(WeatherForecast.Column1))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column2))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column3))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column4))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column5))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column6))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column7))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column8))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column9))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
                },
                new ColumnSettings(nameof(WeatherForecast.Column10))
                {
                    Visible=true,
                    Resizable = true,
                    Width = 200,
                    FrozenType = ColumnFrozenType.None,
                    Sortable = true,
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
