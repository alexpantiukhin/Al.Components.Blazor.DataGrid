using Al.Components.QueryableFilterExpression;

using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class DataRequest<T>
        where T : class
    {
        public FilterExpression<T> FilterExpression { get; set; }
        public Dictionary<string, SortType> Sorts { get; set; }

        public override string ToString() =>
            JsonSerializer.Serialize(this);

        public static DataRequest<T>? ParseJSON(string jsonString) =>
            JsonSerializer.Deserialize<DataRequest<T>>(jsonString);
    }
}
