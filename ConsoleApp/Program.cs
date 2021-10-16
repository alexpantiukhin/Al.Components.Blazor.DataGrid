// See https://aka.ms/new-console-template for more information
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.QueryableFilterExpression;

using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data = new List<A>
            {
                new A{ Id = 1, Name = "Вася"},
                new A{ Id = 2, Name = "Петя"},
                new A{ Id = 3, Name = "Иван"},
                new A{ Id = 4, Name = "Вася"}
            };

            List<Column> columnModelsList = new();
            columnModelsList.Add(new("Column1", x => x.Id, null));
            columnModelsList.Add(new("Column2", x => x.Name, null));


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

            var a = fe.GetExpression(columnModelsList, "x");

            var result = data.AsQueryable().Where(a).ToList();
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

class Column <T>: IFilterExpressionProperty<T>
    where T : class
{
    public string UniqueName { get; set; }
    public Expression<Func<T, object>> Expression { get; set; }
}