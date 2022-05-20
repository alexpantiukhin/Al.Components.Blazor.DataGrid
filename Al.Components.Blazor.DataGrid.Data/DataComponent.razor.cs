using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.ResizeComponent;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Data
{
    public partial class DataComponent : HandRenderComponent
    {
        protected override bool HandRender => true;

        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }


        IResizeAreaComponent ResizeArea;


        bool firstRendered = false;
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                firstRendered = true;


            }
        }
    }
}
