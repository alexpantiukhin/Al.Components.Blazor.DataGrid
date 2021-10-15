// See https://aka.ms/new-console-template for more information
using Al.Components.Blazor.AlDataGrid.Model;
using Al.Components.Blazor.DataGrid.Model;

using System.Linq.Expressions;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<ColumnModel<A>> list = new();
            list.Add(new("Column1", x => x.Id, null));
            list.Add(new("Column2", x => x.Name, null));


            FilterExpression fe = new(FilterExpressionGroupType.Or, new FilterExpression[]
            {
                new FilterExpression(FilterExpressionGroupType.And, new FilterExpression[]
                {
                    new FilterExpression("Column1", FilterOperation.Equal, 2),
                    new FilterExpression("Column2", FilterOperation.Equal, "A")
                }),
                new FilterExpression(FilterExpressionGroupType.And, new FilterExpression[]
                {
                    new FilterExpression("Column1", FilterOperation.Equal, 3),
                    new FilterExpression("Column2", FilterOperation.Equal, "B")
                }),
                new FilterExpression(FilterExpressionGroupType.And, new FilterExpression[]
                {
                    new FilterExpression("Column2", FilterOperation.Equal, "C"),
                    new FilterExpression(FilterExpressionGroupType.Or, new FilterExpression[]
                    {
                        new FilterExpression("Column1", FilterOperation.Equal, 4),
                        new FilterExpression("Column1", FilterOperation.Equal, 5)
                    }),
                }),
            });

            //var fe = new FilterExpression("Column2", FilterOperation.IsNotNull, "2");

            var a = fe.GetExpression(list, "x");


        }
    }
}


class A
{
    public int Id { get; set; }
    public string Name { get; set; }
    public FilterExpression FilterExpression { get; set; }
    public object FIeld { get; set; }
}