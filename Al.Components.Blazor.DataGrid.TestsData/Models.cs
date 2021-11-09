using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Tests.Data;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public static class Models
    {
        public static ColumnsModel<User> AddColumns(ColumnsModel<User> columns)
        {
            {
                ColumnModel<User> _column1 = new(x => x.Id);
                ColumnModel<User> _column2 = new(x => x.FirstName);
                ColumnModel<User> _column3 = new(x => x.LastName);

                columns.All.Add(_column1.UniqueName, _column1);
                columns.All.Add(_column2.UniqueName, _column2);
                columns.All.Add(_column3.UniqueName, _column3);
                columns.All.CompleteAdded();

                return columns;
            }

        }
    }
}
