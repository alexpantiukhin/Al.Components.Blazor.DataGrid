using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_ResizeEnd
    {
        [Fact]
        public async Task NotResizing_ResizingColumnNullAndNotCallEvent()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new());
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnResizeEnd), eventHandler);
            //var column1 = columns.All[0].Value;


            //act
            await columns.ResizeEnd();

            //assert
            Assert.Null(columns.ResizingColumn);
            Assert.False(callEvent);
        }

        [Fact]
        public async Task Resizing_ResizingColumnNotNullAndCallEvent()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new());
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnResizeStart), eventHandler);
            var column2 = columns.All[1];
            
            //act
            await columns.ResizeStart(column2, 0);

            // assert
            Assert.Equal(column2, columns.ResizingColumn);

            //act
            await columns.ResizeEnd();

            //assert
            Assert.Null(columns.ResizingColumn);
            Assert.True(callEvent);
        }
    }
}
