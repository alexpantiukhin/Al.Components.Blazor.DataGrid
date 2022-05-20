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
    public partial class Body : HandRenderComponent
    {
        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await DataGridModel.Data.Refresh();
        }


        static string GetData(object model, ColumnModel column)
        {
            var type = model.GetType();

            var property = type.GetProperty(column.UniqueName);

            if(property == null)
                return "&nbsp";

            var value = property.GetValue(model);

            return value?.ToString() ?? "";
        }
    }
}
