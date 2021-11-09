using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_DragColumnStart
    {
        [Fact]
        public async Task Draggable_DraggingColumnNotNullAndEventCall()
        {
            //arrange
            bool callEvent = false;
            ColumnsModel<User> columns = Models.AddColumns(new() { Draggable = true});
            Func<ColumnModel<User>, Task> eventHandler = async (x) => callEvent = true;
            EventTest<ColumnsModel<User>> eventTest = new(columns, nameof(columns.OnDragStart), eventHandler);
            var column2 = columns.All[1].Value;


            //act
            await columns.DragColumnStart(column2);

            //assert
            Assert.Equal(column2, columns.DraggingColumn);
            Assert.True(callEvent);
        }

        [Fact]
        public async Task NotDraggable_DraggingColumnNullAndEventNotCall()
        {
            //arrange
            bool callEvent = false;
            ColumnsModel<User> columns = Models.AddColumns(new());
            Func<ColumnModel<User>, Task> eventHandler = async (x) => callEvent = true;
            EventTest<ColumnsModel<User>> eventTest = new(columns, nameof(columns.OnDragStart), eventHandler);
            var column2 = columns.All[1].Value;

            //act
            await columns.DragColumnStart(column2);

            //assert
            Assert.Null(columns.DraggingColumn);
            Assert.False(callEvent);
        }
    }
}
