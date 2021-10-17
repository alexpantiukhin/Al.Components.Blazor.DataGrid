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

            FilterExpression<A> fe = FilterExpression<A>.GroupOr(
                            new(nameof(A.Id), FilterOperation.Equal, 1),
                            new(nameof(A.Name), FilterOperation.StartWith, "Пе"));

            var a = fe.GetExpression("x");

            var e = data.AsQueryable().Where(a).ToArray();

            var str = fe.ToString();

            Console.WriteLine(str);

            FilterExpression<A> b = FilterExpression<A>.ParseJSON(str);

            Expression<Func<A, bool>> c = b.GetExpression("x");

            var d = data.AsQueryable().Where(c).ToArray();
        }
    }
}


class A
{
    public int Id { get; set; }
    public string Name { get; set; }
    public object FIeld { get; set; }
}