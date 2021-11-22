using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Header
{
    public partial class HeaderColumn<T> : HandRenderComponent
        where T : class
    {
        [Parameter]
        [EditorRequired]
        public DataGridModel<T> DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public ColumnModel<T> ColumnModel {  get; set;}
    }
}
