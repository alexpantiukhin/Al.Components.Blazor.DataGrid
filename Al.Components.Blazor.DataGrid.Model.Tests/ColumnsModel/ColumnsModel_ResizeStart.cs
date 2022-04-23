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
    public class ColumnsModel_ResizeStart
    {
        [Fact]
        public async Task NotResizable_ResizingColumnNullAndNotCallEvent()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new());
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnResizeStart), eventHandler);
            var column1 = columns.All[0].Item;


            //act
            await columns.ResizeStart(column1);

            //assert
            Assert.Null(columns.ResizingColumn);
            Assert.False(callEvent);
        }

        [Fact]
        public async Task Resizable_ResizingColumnNotNullAndCallEvent()
        {
            //arrange
            bool callEvent = false;
            Model.ColumnsModel columns = Models.AddColumns(new());
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnResizeStart), eventHandler);
            var column2 = columns.All[1].Item;

            //act
            await columns.ResizeStart(column2);

            //assert
            Assert.Equal(column2, columns.ResizingColumn);
            Assert.True(callEvent);
        }
    }
}
