using Al.Collections;
using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public class ColumnModel_ChangeProperties
    {


        [Fact]
        public void VisibleSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Visible), true,
                false, nameof(ColumnModel.VisibleChange), nameof(column.OnVisibleChanged), false);

            Assert.True(columns.VisibleNotify);
        }

        [Fact]
        public void SortableSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Sortable), false,
                true, nameof(ColumnModel.SortableChange), nameof(column.OnSortableChanged), true);
        }

        [Fact]
        public void WidthSetter_LessMinimum_ReturnMinimum()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Width),
                ColumnModel.DefaultWidth, ColumnModel.MinWidth - 10,
                nameof(ColumnModel.WidthChange), nameof(column.OnWidthChanged),
                ColumnModel.MinWidth);
        }

        [Fact]
        public void WidthSetter_MoreMinimum_ReturnMoreMinimum()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Width),
                ColumnModel.DefaultWidth, ColumnModel.MinWidth + 10,
                nameof(ColumnModel.WidthChange), nameof(column.OnWidthChanged),
                ColumnModel.MinWidth + 10);
        }

        [Fact]
        public void TitleSetter_ExpressionTitleEmpty_EqualPropName()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));
            var testTitle = "testTitle";

            TestHelper.TestAsynSetter(column, nameof(column.Title),
                nameof(User.Id), testTitle,
                nameof(ColumnModel.TitleChange), nameof(column.OnTitleChanged),
                testTitle);
        }

        [Fact]
        public void SortSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Sort),
                null,(SortDirection?) SortDirection.Ascending, nameof(column.SortChange),
                nameof(column.OnSortChanged), SortDirection.Ascending);

            Assert.True(columns.SortNotify);
        }

        [Fact]
        public void ResizableSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Resizable),
                false, true, nameof(column.ResizeableChange),
                nameof(column.OnResizeableChanged), true);
        }

        [Fact]
        public void FixedTypeSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.FixedType),
                ColumnFixedType.None, ColumnFixedType.Left, nameof(column.FixedTypeChange),
                nameof(column.OnFixedTypeChanged), ColumnFixedType.Left);

            Assert.True(columns.FixedTypeNotify);
        }

        [Fact]
        public void FilterableSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));

            TestHelper.TestAsynSetter(column, nameof(column.Filterable),
                false, true, nameof(column.FilterableChange),
                nameof(column.OnFilterableChanged), true);
        }

        [Fact]
        public void FilterSetter()
        {
            //arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));
            var filterExpression = new RequestFilter(nameof(User.Id), FilterOperation.Equal, "1");

            TestHelper.TestAsynSetter(column, nameof(column.Filter),
                null, filterExpression, nameof(column.FilterChange),
                nameof(column.OnFilterChanged), filterExpression);

            Assert.True(columns.FilterNotify);
        }

    }
}
