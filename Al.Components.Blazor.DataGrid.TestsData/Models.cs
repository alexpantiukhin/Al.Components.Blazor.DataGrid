using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Tests.Data;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public static class Models
    {
        public static ColumnsModel AddColumns(ColumnsModel columns)
        {
                ColumnModel _column1 = new(columns, nameof(User.Id));
                ColumnModel _column2 = new(columns, nameof(User.FirstName)) {  Resizable = true };
                ColumnModel _column3 = new(columns, nameof(User.LastName));

                columns.All.Add(_column1.UniqueName, _column1);
                columns.All.Add(_column2.UniqueName, _column2);
                columns.All.Add(_column3.UniqueName, _column3);
                columns.All.CompleteAdded();

                return columns;
        }
    }
}
