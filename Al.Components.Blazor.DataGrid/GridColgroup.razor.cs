using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid
{
    public partial class GridColgroup<T> : HandRenderComponent, IDisposable
        where T : class
    {
        protected override bool HandRender => true;

        [Parameter] 
        public DataGridModel<T> DataGridModel { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            DataGridModel.Columns.OnResizing += OnResizeStartHandler;
        }


        Task OnResizeStartHandler(ColumnModel<T> args)
        {
            return RenderAsync();
        }

        void IDisposable.Dispose()
        {
            DataGridModel.Columns.OnResizeStart -= OnResizeStartHandler;
        }
    }
}
