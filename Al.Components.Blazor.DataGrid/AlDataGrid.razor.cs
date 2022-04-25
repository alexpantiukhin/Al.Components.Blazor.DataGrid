using Al.Collections.Api;
using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid
{
    public partial class AlDataGrid : HandRenderComponent
    {
        protected override bool HandRender => true;

        [Parameter]
        public RenderFragment Columns { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public IEnumerable Items
        {
            get => _items;
            set
            {
                _items = value;
                if (_model != null)
                    _model.Data.Items = value;
            }
        }

        [Parameter]
        public Func<CollectionRequest, CancellationToken, Task<CollectionResponse>> GetDataAsync { get; set; }


        DataGridModel _model;

        private IEnumerable _items;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (GetDataAsync != null)
                _model = new DataGridModel(GetDataAsync);
            else
            {
                _model = new();
                _model.Data.Items = _items;
            }
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
