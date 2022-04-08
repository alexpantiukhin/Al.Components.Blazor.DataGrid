using Al.Collections.ExpressionExtensions;
using Al.Collections.QueryableFilterExpression;

using System.ComponentModel;
using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class DataRequest<T>
        where T : class
    {
        public HashSet<string> Grouping { get; set; } = new();
        public FilterExpression<T>? FilterExpression { get; set; }
        public Dictionary<string, ListSortDirection> Sorts { get; set; } = new();

        public override string ToString() =>
            JsonSerializer.Serialize(this);

        public static DataRequest<T>? ParseJSON(string jsonString) =>
            JsonSerializer.Deserialize<DataRequest<T>>(jsonString);

        public IQueryable<T> Apply(IEnumerable<T> source, IOperationExpressionResolver operationExpressionResolver)
        {
            IQueryable<T> result = source is IQueryable<T> queryableList ? queryableList : source.AsQueryable();

            if (FilterExpression is not null)
                result = result.Where(FilterExpression.GetExpression("x", operationExpressionResolver));

            if (Sorts?.Count > 0)
                result = result.Orders(Sorts);

            return result;
        }
    }
}
