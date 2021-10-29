using Al.Collections.ExpressionExtensions;
using Al.Components.QueryableFilterExpression;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class DataRequest<T>
        where T : class
    {
        public FilterExpression<T>? FilterExpression { get; set; }
        public Dictionary<string, ListSortDirection>? Sorts { get; set; }

        public override string ToString() =>
            JsonSerializer.Serialize(this);

        public static DataRequest<T>? ParseJSON(string jsonString) =>
            JsonSerializer.Deserialize<DataRequest<T>>(jsonString);

        public IQueryable<T> Apply(IQueryable<T> source)
        {
            var result = source;
            if (FilterExpression is not null)
                result = result.Where(FilterExpression.GetExpression("x"));

            if (Sorts?.Count > 0)
                result = result.Orders(Sorts);

            return result;
        }
    }
}
