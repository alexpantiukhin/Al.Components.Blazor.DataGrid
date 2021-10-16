// See https://aka.ms/new-console-template for more information
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

            List<Column<A>> columnModelsList = new();
            columnModelsList.Add(new(nameof(A.Id), x => x.Id));
            columnModelsList.Add(new(nameof(A.Name), x => x.Name));


            var fe = FilterExpression<A>.GroupOr(
                        FilterExpression<A>.GroupAnd(
                            new(nameof(A.Id), FilterOperation.Equal, 2),
                            new(nameof(A.Name), FilterOperation.Equal, "A")),
                        FilterExpression<A>.GroupAnd(
                            new(nameof(A.Id), FilterOperation.Equal, 3),
                            new(nameof(A.Name), FilterOperation.Equal, "B")),
                        FilterExpression<A>.GroupAnd(
                            new(nameof(A.Name), FilterOperation.Equal, "C"),
                            FilterExpression<A>.GroupOr(
                                new(nameof(A.Id), FilterOperation.Equal, 4),
                                new(nameof(A.Id), FilterOperation.Equal, 5))));

            var a = fe.GetExpression(columnModelsList, "x");

            var result = data.AsQueryable().Where(a).ToList();
        }
    }
}


class A
{
    public int Id { get; set; }
    public string Name { get; set; }
    public object FIeld { get; set; }
}

class Column<T> : IFilterExpressionProperty<T>
    where T : class
{
    public Column(string uniqueName, Expression<Func<T, object>> expression)
    {
        UniqueName = uniqueName ?? throw new ArgumentNullException(nameof(uniqueName));
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public string UniqueName { get; set; }
    public Expression<Func<T, object>> Expression { get; set; }
}