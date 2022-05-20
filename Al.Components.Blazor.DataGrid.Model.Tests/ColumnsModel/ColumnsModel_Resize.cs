using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_Resize
    {
        [Fact]
        public async Task NullOffset_WidthEqualStart()
        {
            //arrange
            Model.ColumnsModel columns = Models.AddColumns(new());
            var column2 = columns.All[1];
            await columns.ResizeStart(column2, 0);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var expectedXResizer = leftBorderHeadX + startWidth;
            bool callEvent = false;
            Func<ColumnModel, Task> eventHandler = async (x) => callEvent = true;
            EventTest<Model.ColumnsModel> eventTest = new(columns, nameof(columns.OnResizing), eventHandler);


            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth);


            //assert
            Assert.Equal(startWidth, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
            Assert.True(callEvent);
        }

        [Fact]
        public async Task TableMode_PlusOffset_WidthMoreStart()
        {
            //arrange
            Model.ColumnsModel columns = Models.AddColumns(new());
            var column2 = columns.All[1];
            await columns.ResizeStart(column2, 0);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var offset = 10;
            var expectedXResizer = leftBorderHeadX + startWidth + offset;

            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth + offset);


            //assert
            Assert.Equal(startWidth + offset, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
        }

        [Fact]
        public async Task TableMode_MinusOffsetMoreMinimum_WidthLessStart()
        {
            //arrange
            Model.ColumnsModel columns = Models.AddColumns(new());
            var column2 = columns.All[1];
            await columns.ResizeStart(column2, 0);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var offset = -10;
            var expectedXResizer = leftBorderHeadX + startWidth + offset;

            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth + offset);


            //assert
            Assert.Equal(startWidth + offset, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
        }

        [Fact]
        public async Task TableMode_MinusOffsetLessMinimum_WidthEqualMinimum()
        {
            //arrange
            Model.ColumnsModel columns = Models.AddColumns(new());
            var column2 = columns.All[1];
            await columns.ResizeStart(column2, 0);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var offset = -100;
            var expectedXResizer = leftBorderHeadX + ColumnModel.MIN_WIDTH;

            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth + offset);

            //assert
            Assert.Equal(ColumnModel.MIN_WIDTH, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
        }

        [Fact]
        public async Task SiblingMode_PlusOffset_WidthMoreStart()
        {
            //arrange
            Model.ColumnsModel columns = Models.AddColumns(new());
            columns.ResizeMode = ResizeMode.Sibling;
            var column2 = columns.All[1];
            await columns.ResizeStart(column2, 0);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var offset = 10;
            var expectedXResizer = leftBorderHeadX + startWidth + offset;

            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth + offset);


            //assert
            Assert.Equal(startWidth + offset, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
        }

        [Fact]
        public async Task SiblingMode_PlusOffsetMoreSiblingWidth_WidthStartPlusFreeSibling()
        {
            //arrange
            Model.ColumnsModel columns = Models.AddColumns(new());
            columns.ResizeMode = ResizeMode.Sibling;
            var column2 = columns.All[1];
            var column3 = columns.All[2];

            await columns.ResizeStart(column2, 0);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var siblingFreeSpace = column3.Width - ColumnModel.MIN_WIDTH;
            var offset = 200;
            var expectedXResizer = leftBorderHeadX + startWidth + siblingFreeSpace;

            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth + offset);

            //assert
            Assert.Equal(startWidth + siblingFreeSpace, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
        }
    }
}
