// See https://aka.ms/new-console-template for more information
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Data;
using Al.Components.QueryableFilterExpression;

using ConsoleApp.DB;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel;

namespace ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            var query = db.Bs.Include(x => x.A)
                .Where(x => x.A.Name == "Вася")
                .Select(x => new ViewModel
                {
                    Id = x.Id,
                    Name = x.A.Name,
                    SubName = x.BName
                })
                .OrderBy(x => x.SubName);

            SimpleSqlDataProvider<ViewModel> dataProvider = new(query);

            DataGridModel<ViewModel> dataGridModel = new(dataProvider);

            var idColumn = new ColumnModel<ViewModel>(x => x.Id)
            {
                Sortable = true,
                Filterable = true,
                Sort = ListSortDirection.Ascending
            };
            var nameColumn = new ColumnModel<ViewModel>(x => x.Name)
            {
                Sortable = true,
                Filterable = true,
                Sort = ListSortDirection.Ascending
            };

            dataGridModel.Columns.Add(idColumn);

            dataGridModel.Columns.Add(nameColumn);


            await dataGridModel.RefreshData();

            idColumn.Sort = ListSortDirection.Descending;

            await dataGridModel.RefreshData();

            idColumn.FilterExpression = new FilterExpression<ViewModel>(idColumn.UniqueName, FilterOperation.Equal, 1);

            await dataGridModel.Filter.SerExpressionByColumns(dataGridModel.Columns.All.Where(x => x.Value.Filterable).Select(x => x.Value));

            await dataGridModel.Filter.ToggleApplyFilter();

            var a = 1;
        }
    }
}
