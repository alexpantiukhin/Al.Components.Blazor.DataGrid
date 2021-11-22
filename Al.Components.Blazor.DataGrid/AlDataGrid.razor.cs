using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid
{
    public partial class AlDataGrid<T> : HandRenderComponent, IDisposable
        where T : class
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public RenderFragment Columns { get; set; }


        [Parameter]
        [EditorRequired]
        public IDataProvider<T> DataProvider { get; set; }

        [Parameter]
        [EditorRequired]
        public IOperationExpressionResolver OperationExpressionResolver { get; set; }

        [Parameter]
        public string CssClass { get; set; }


        DataGridModel<T> _model;

        bool _columnsAdded;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _model = new DataGridModel<T>(DataProvider, OperationExpressionResolver);
            _model.Columns.All.OnAddCompleted += ColumnsAddedHandler;
        }

        void ColumnsAddedHandler()
        {
            _columnsAdded = true;
            Render();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
                _model.Columns.All.CompleteAdded();
        }

        public void Dispose()
        {
            _model.Columns.All.OnAddCompleted -= ColumnsAddedHandler;
        }
    }
}
