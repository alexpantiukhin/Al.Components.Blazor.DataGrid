using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_ReorderColumnStart
    {
        ColumnModel<User> _column1 = new(x => x.Id);
        ColumnModel<User> _column2 = new(x => x.FirstName);
        ColumnModel<User> _column3 = new(x => x.LastName);

        ColumnsModel<User> AddColumns(ColumnsModel<User> columns)
        {
            columns.All.Add(_column1.UniqueName, _column1);
            columns.All.Add(_column2.UniqueName, _column2);
            columns.All.Add(_column3.UniqueName, _column3);
            columns.All.CompleteAdded();

            return columns;
        }

        [Fact]
        public async Task Draggable_DraggingColumnNotNullAndEventCall()
        {
            //arrange
            bool callEvent = false;
            ColumnsModel<User> columns = AddColumns(new() { Draggable = true});
            Func<ColumnModel<User>, Task> eventHandler = async (x) => callEvent = true;
            EventTest<ColumnsModel<User>> eventTest = new(columns, nameof(columns.OnDragStart), eventHandler);


            //act
            await columns.DragColumnStart(_column2);

            //assert
            Assert.Equal(_column2, columns.DraggingColumn);
            Assert.True(callEvent);
        }

        [Fact]
        public async Task NotDraggable_DraggingColumnNullAndEventNotCall()
        {
            //arrange
            bool callEvent = false;
            ColumnsModel<User> columns = AddColumns(new());
            Func<ColumnModel<User>, Task> eventHandler = async (x) => callEvent = true;
            EventTest<ColumnsModel<User>> eventTest = new(columns, nameof(columns.OnDragStart), eventHandler);

            //act
            await columns.DragColumnStart(_column2);

            //assert
            Assert.Null(columns.DraggingColumn);
            Assert.False(callEvent);
        }
    }
}
