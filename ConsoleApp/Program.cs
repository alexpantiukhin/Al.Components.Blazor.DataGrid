// See https://aka.ms/new-console-template for more information
using Al.Components.Blazor.DataGrid.Model;

using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var model1 = new Model
            {
                Id = 1,
                Name = "Model1",
            };
            var model2 = new Model
            {
                Name = "Model2",
                Id = 2,
            };


            var subModel1 = new SubModel
            {
                Id = 1,
                ParentModel = model1,
                SubModelName = "SubModel1"
            };

            var subModel2 = new SubModel
            {
                Id = 2,
                ParentModel = model2,
                SubModelName = "SubModel2"
            };

            var subModel3 = new SubModel
            {
                Id = 3,
                ParentModel = model1,
                SubModelName = "SubModel3"
            };

            var context = new ListContext
            {
                SubModels = new List<SubModel> { subModel1, subModel2, subModel3 },
                Models = new List<Model> { model1, model2 }
            };



            ListDataProvider dataProvider = new(context);
            DataGridModel<ViewModel> gridModel = new(dataProvider);
            gridModel.Columns.Add(new ColumnModel<ViewModel>(x => x.Id, null)
            {
                ShowName = "№"
            });
            gridModel.Columns.Add(new ColumnModel<ViewModel>(x => x.Name, null)
            {
                ShowName = "Родитель"
            });
            gridModel.Columns.Add(new ColumnModel<ViewModel>(x => x.SubName, null)
            {
                ShowName = "Имя"
            });


        }
    }

}
