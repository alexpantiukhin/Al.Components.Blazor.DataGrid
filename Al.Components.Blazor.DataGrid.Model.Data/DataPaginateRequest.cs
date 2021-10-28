using System.Text.Json;

namespace Al.Components.Blazor.DataGrid.Model.Data
{
    public class DataPaginateRequest<T> : DataRequest<T>
        where T : class
    {
        public int Skip { get; set; }
        public int Take { get; set; }

        public override string ToString() =>
            JsonSerializer.Serialize(this);

        public static DataPaginateRequest<T>? ParseJSON(string jsonString) =>
            JsonSerializer.Deserialize<DataPaginateRequest<T>>(jsonString);
    }
}
