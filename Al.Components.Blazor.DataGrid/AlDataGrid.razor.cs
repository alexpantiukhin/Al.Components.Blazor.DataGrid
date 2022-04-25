using Al.Collections.Api;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Components.Blazor.HandRender;
using Al.Helpers.Throws;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid
{
    public partial class AlDataGrid : HandRenderComponent
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public Func<CancellationToken, Task<SettingsModel>> GetSettings { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        [EditorRequired]
        public Func<CollectionRequest, CancellationToken, Task<CollectionResponse>> GetDataAsync { get; set; }


        DataGridModel _model;

        IEnumerable _items;

        IEnumerable<ColumnModel> _columns;

        protected override async void OnInitialized()
        {
            base.OnInitialized();

            ParametersThrows.ThrowIsNull(GetSettings, nameof(GetSettings));
            ParametersThrows.ThrowIsNull(GetDataAsync, nameof(GetDataAsync));

            _model = new DataGridModel(GetDataAsync);

            var settings = await GetSettings.Invoke(default);

            _model.ApplySettings(settings);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _model.Columns.CompleteAddedColumns();
                await RenderAsync();
            }
        }
    }
}
