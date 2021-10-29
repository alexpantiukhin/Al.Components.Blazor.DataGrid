// See https://aka.ms/new-console-template for more information
using Al.Components.Blazor.DataGrid.Model.Data;

using ConsoleApp.DB;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new TestDbContext();

            //var @as = new List<A>
            //{
            //    new A{ Id = 1, Name = "Вася"},
            //    new A{ Id = 2, Name = "Петя"},
            //    new A{ Id = 3, Name = "Иван"},
            //    new A{ Id = 4, Name = "Вася"}
            //};

            //db.As.AddRange(@as);

            //var bs = new List<B>
            //{
            //    new B{ Id = 1, A = @as[0], BName = "BName Вася"},
            //    new B{ Id = 2, A = @as[1], BName = "BName Петя"},
            //    new B{ Id = 3, A = @as[2], BName = "BName Иван"},
            //    new B{ Id = 4, A = @as[3], BName = "BName Вася"},
            //};

            //db.Bs.AddRange(bs);


            //db.SaveChanges();
            //db.Bs.CountAsync();
            var query = db.Bs.Include(x => x.A).Select(x => new ViewModel
            {
                Id = x.Id,
                Name = x.A.Name,
                SubName = x.BName
            });
            SimpleSqlDataProvider<ViewModel> dataProvider = new(query);
            DataModel<ViewModel> dataModel = new(dataProvider);



            dataModel.RefreshData(new()
            {
                Skip = 1,
                Sorts = new() { { nameof(ViewModel.Name), ListSortDirection.Descending }, { nameof(ViewModel.SubName), ListSortDirection.Descending } }
            });


        }
    }
}
