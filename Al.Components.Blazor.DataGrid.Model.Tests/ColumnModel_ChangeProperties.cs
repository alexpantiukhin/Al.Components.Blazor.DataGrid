using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Tests.Data;

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
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Visible), true,
                false, nameof(ColumnModel<User>.VisibleChange), nameof(column.OnVisibleChanged), false);
        }

        [Fact]
        public void SortableSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Sortable), false,
                true, nameof(ColumnModel<User>.SortableChange), nameof(column.OnSortableChanged), true);
        }

        [Fact]
        public void WidthSetter_LessMinimum_ReturnMinimum()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Width),
                ColumnModel<User>.DefaultWidth, ColumnModel<User>.MinWidth - 10,
                nameof(ColumnModel<User>.WidthChange), nameof(column.OnWidthChanged),
                ColumnModel<User>.MinWidth);
        }

        [Fact]
        public void WidthSetter_MoreMinimum_ReturnMoreMinimum()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Width),
                ColumnModel<User>.DefaultWidth, ColumnModel<User>.MinWidth + 10,
                nameof(ColumnModel<User>.WidthChange), nameof(column.OnWidthChanged),
                ColumnModel<User>.MinWidth + 10);
        }

        [Fact]
        public void TitleSetter_ExpressionTitleEmpty_EqualPropName()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);
            var testTitle = "testTitle";

            TestHelper.TestAsynSetter(column, nameof(column.Title),
                nameof(User.Id), testTitle,
                nameof(ColumnModel<User>.TitleChange), nameof(column.OnTitleChanged),
                testTitle);
        }

        [Fact]
        public void SortSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Sort),
                null,(ListSortDirection?) ListSortDirection.Ascending, nameof(column.SortChange),
                nameof(column.OnSortChanged), ListSortDirection.Ascending);
        }

        [Fact]
        public void ResizableSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Resizable),
                false, true, nameof(column.ResizeableChange),
                nameof(column.OnResizeableChanged), true);
        }

        [Fact]
        public void FixedTypeSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.FixedType),
                ColumnFixedType.None, ColumnFixedType.Left, nameof(column.FixedTypeChange),
                nameof(column.OnFixedTypeChanged), ColumnFixedType.Left);
        }

        [Fact]
        public void DragableSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Draggable),
                false, true, nameof(column.DraggableChange),
                nameof(column.OnDraggableChanged), true);
        }

        [Fact]
        public void FilterableSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Filterable),
                false, true, nameof(column.FilterableChange),
                nameof(column.OnFilterableChanged), true);
        }

        [Fact]
        public void FilterSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);
            var filterExpression = new FilterExpression<User>(nameof(User.Id), FilterOperation.Equals, 1);

            TestHelper.TestAsynSetter(column, nameof(column.Filter),
                null, filterExpression, nameof(column.FilterChange),
                nameof(column.OnFilterChanged), filterExpression);
        }

    }
}
