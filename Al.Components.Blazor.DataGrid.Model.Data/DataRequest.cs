using Al.Collections.ExpressionExtensions;
using Al.Components.QueryableFilterExpression;

using System.ComponentModel;
using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class DataRequest<T>
        where T : class
    {
        public HashSet<string>? Grouping { get; set; }
        public FilterExpression<T>? FilterExpression { get; set; }
        public Dictionary<string, ListSortDirection>? Sorts { get; set; }

        public override string ToString() =>
            JsonSerializer.Serialize(this);

        public static DataRequest<T>? ParseJSON(string jsonString) =>
            JsonSerializer.Deserialize<DataRequest<T>>(jsonString);

        public IQueryable<T> Apply(IEnumerable<T> source)
        {
            IQueryable<T> result = null;

            if(source is IQueryable<T> queryableList)
            {
                if (FilterExpression is not null)
                    result = queryableList.Where(FilterExpression.GetExpression("x"));
                else
                    result = queryableList;

                if (Sorts?.Count > 0)
                    result = queryableList.SqlOrders(Sorts);
            }
            else
            {
                if (FilterExpression is not null)
                    result = source.AsQueryable().Where(FilterExpression.GetExpression("x"));
                else
                    result = source.AsQueryable();

                if (Sorts?.Count > 0)
                    result = source.AsQueryable().SqlOrders(Sorts);
            }

            return result;
        }
    }
}
