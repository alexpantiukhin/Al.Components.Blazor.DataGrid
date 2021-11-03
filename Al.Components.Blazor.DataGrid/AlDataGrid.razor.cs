using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

namespace Al.Components.Blazor.DataGrid
{
    public partial class AlDataGrid<T> : HandRenderComponent
        where T : class
    {
        [Parameter]
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



        protected override void OnInitialized()
        {
            base.OnInitialized();

            _model = new Model.DataGridModel<T>(DataProvider, OperationExpressionResolver);


        }
    }
}
