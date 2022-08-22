using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_DragColumnEnd
    {
        [Fact]
        public async Task DropSelf_DraggingColumnNullAndNotEventCall()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new() { Draggable = true });
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnDragEnd), eventHandler);
            var column2 = columns.All[1];
            await columns.DragColumnStart(column2);

            //act
            await columns.DragColumnEnd(column2, true);

            //assert
            Assert.Null(columns.DraggingColumn);
            Assert.False(callEvent);
        }


        [Fact]
        public async Task DropBeforeNotSelf_DraggingColumnNullAndEventCallAndColumnBefore()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new() { Draggable = true });
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnDragEnd), eventHandler);
            var column1node = columns.All[0];
            var column2node = columns.All[1];
            await columns.DragColumnStart(column2node);

            //act
            await columns.DragColumnEnd(column1node, true);
            var column1Index = Array.IndexOf(columns.All, column1node);
            var column2Index = Array.IndexOf(columns.All, column2node);

            //assert
            Assert.Null(columns.DraggingColumn);
            Assert.True(callEvent);
            Assert.True(column1Index - 1 == column2Index);
        }

        [Fact]
        public async Task DropAfterNotSelf_DraggingColumnNullAndEventCallAndColumnBefore()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new() { Draggable = true });
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnDragEnd), eventHandler);
            var column3node = columns.All[0];
            var column2node = columns.All[1];
            await columns.DragColumnStart(column2node);

            //act
            await columns.DragColumnEnd(column3node, false);
            var column2Index = Array.IndexOf(columns.All, column2node);
            var column3Index = Array.IndexOf(columns.All, column3node);

            //assert
            Assert.Null(columns.DraggingColumn);
            Assert.True(callEvent);
            Assert.True(column2Index - 1 == column3Index);
        }
    }
}
