using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Header
{
    public partial class HeaderColumn<T> : HandRenderComponent, IDisposable
        where T : class
    {
        [Parameter]
        [EditorRequired]
        public DataGridModel<T> DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public ColumnModel<T> ColumnModel {  get; set;}

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ColumnModel.OnSortChanged += RenderAsync;
        }


        public async Task ClickSortHandler()
        {
            ListSortDirection? newValue;
            if (ColumnModel.Sort is null)
                newValue = ListSortDirection.Ascending;
            else if (ColumnModel.Sort == ListSortDirection.Ascending)
                newValue = ListSortDirection.Descending;
            else
                newValue = null;

            await ColumnModel.SortChange(newValue);
        }

        public void Dispose()
        {
            ColumnModel.OnSortChanged -= RenderAsync;
        }
    }
}
