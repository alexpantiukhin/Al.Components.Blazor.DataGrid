using Al.Components.Blazor.DataGrid.Model.Enums;
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
    public class ColumnsModel_Resize
    {
        [Fact]
        public async Task NullOffset_WidthEqualStart()
        {
            //arrange
            ColumnsModel<User> columns = Models.AddColumns(new());
            var column2 = columns.All[1].Value;
            await columns.ResizeStart(column2);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var expectedXResizer = leftBorderHeadX + startWidth;
            bool callEvent = false;
            Func<ColumnModel<User>, Task> eventHandler = async (x) => callEvent = true;
            EventTest<ColumnsModel<User>> eventTest = new(columns, nameof(columns.OnResizing), eventHandler);


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
            ColumnsModel<User> columns = Models.AddColumns(new());
            var column2 = columns.All[1].Value;
            await columns.ResizeStart(column2);
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
            ColumnsModel<User> columns = Models.AddColumns(new());
            var column2 = columns.All[1].Value;
            await columns.ResizeStart(column2);
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
            ColumnsModel<User> columns = Models.AddColumns(new());
            var column2 = columns.All[1].Value;
            await columns.ResizeStart(column2);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var offset = -100;
            var expectedXResizer = leftBorderHeadX + ColumnModel<User>.MinWidth;

            //act
            var xResizer = await columns.Resize(leftBorderHeadX, leftBorderHeadX + startWidth + offset);

            //assert
            Assert.Equal(ColumnModel<User>.MinWidth, column2.Width);
            Assert.Equal(expectedXResizer, xResizer);
        }

        [Fact]
        public async Task SiblingMode_PlusOffset_WidthMoreStart()
        {
            //arrange
            ColumnsModel<User> columns = Models.AddColumns(new());
            columns.ResizeMode = ResizeMode.Sibling;
            var column2 = columns.All[1].Value;
            await columns.ResizeStart(column2);
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
            ColumnsModel<User> columns = Models.AddColumns(new());
            columns.ResizeMode = ResizeMode.Sibling;
            var column2 = columns.All[1].Value;
            var column3 = columns.All[2].Value;

            await columns.ResizeStart(column2);
            var startWidth = column2.Width;
            var leftBorderHeadX = 10;
            var siblingFreeSpace = column3.Width - ColumnModel<User>.MinWidth;
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
