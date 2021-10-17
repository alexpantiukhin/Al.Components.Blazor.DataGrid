// See https://aka.ms/new-console-template for more information
using Al.Components.QueryableFilterExpression;

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
                            new(nameof(A.Name), FilterOperation.StartWith, "Ив"),
                            new(nameof(A.Name), FilterOperation.StartWith, "Пе"));

            var a = fe.GetExpression("x");

            var str = fe.ToString();

            Console.WriteLine(str);

            var b = FilterExpression<A>.ParseJSON(str);

        }
    }
}


class A
{
    public int Id { get; set; }
    public string Name { get; set; }
    public object FIeld { get; set; }
}