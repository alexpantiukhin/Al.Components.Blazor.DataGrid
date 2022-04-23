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
            Model.ColumnsModel columns = Models.AddColumns(new() { Draggable = true});
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnDragStart), eventHandler);
            var column2 = columns.All[1].Item;


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
            Model.ColumnsModel columns = Models.AddColumns(new());
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnDragStart), eventHandler);
            var column2 = columns.All[1].Item;

            //act
            await columns.DragColumnStart(column2);

            //assert
            Assert.Null(columns.DraggingColumn);
            Assert.False(callEvent);
        }
    }
}
