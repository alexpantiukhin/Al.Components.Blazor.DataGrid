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
    public partial class GridHeader<T> : HandRenderComponent
        where T : class
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public DataGridModel<T> DataGridModel { get; set; }
    }
}
