using Al.Components.Blazor.DataGrid.Model;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.DataComponent
{
    public partial class Data : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

        [Parameter]
        public string UniqueKeyResizeArea { get;set; }
    }
}
