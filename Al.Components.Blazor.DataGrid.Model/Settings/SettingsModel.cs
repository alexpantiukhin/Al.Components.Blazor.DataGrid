using Al.Components.QueryableFilterExpression;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Model.Settings
{
    public class SettingsModel<T>
        where T : class
    {
        public List<ColumnSettings<T>> Columns { get; set; }

        public FilterExpression<T> ConstructorFilterExpression {  get; set; }

        public bool FilterApplied { get; set; }
        public HashSet<string> Grouping { get; set; }


    }
}
