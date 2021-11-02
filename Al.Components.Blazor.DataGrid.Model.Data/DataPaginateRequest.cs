using Al.Collections.QueryableFilterExpression;

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

        public static new DataPaginateRequest<T>? ParseJSON(string jsonString) =>
            JsonSerializer.Deserialize<DataPaginateRequest<T>>(jsonString);

        public IQueryable<T> Apply(IQueryable<T> source, IOperationExpressionResolver operationExpressionResolver)
        {
            var result = base.Apply(source, operationExpressionResolver);

            if (Skip > 0)
                result = result.Skip(Skip);

            if(Take > 0)
                result =  result.Take(Take);

            return result;
        }
    }
}
