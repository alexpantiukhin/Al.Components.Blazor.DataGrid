using Al.Components.Blazor.DataGrid.TestsData;

using System;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public class ColumnModel_Constructor
    {
        [Fact]
        public void CreateByUniqueName_HasUniqueName()
        {
            //arrange
            var columns = new TestColumns();
            string name = "column";

            //act
            var column = new ColumnModel(columns, name);

            //assert
            Assert.Equal(name, column.UniqueName);
        }

        [Fact]
        public void CreateByNullUniqueName_Exception()
        {
            //arrange
            var columns = new TestColumns();

            Assert.Throws<ArgumentNullException>(() => new ColumnModel(columns, string.Empty));
        }

        //[Fact]
        //public void CreateWidthLessMinimal_WidthMinimal()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression)
        //    {
        //        Width = ColumnModel<User>.MinWidth - 10
        //    };

        //    //assert
        //    Assert.Equal(ColumnModel<User>.MinWidth, column.Width);
        //}

        //[Fact]
        //public void Create_DefaultVisibleTrue()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.True(column.Visible);
        //}

        //[Fact]
        //public void Create_DefaulSortableFalse()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.False(column.Sortable);
        //}

        //[Fact]
        //public void Create_DefaulDraggableFalse()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.False(column.Draggable);
        //}

        //[Fact]
        //public void Create_DefaulFilterableFalse()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.False(column.Filterable);
        //}

        //[Fact]
        //public void Create_DefaulResizableFalse()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.False(column.Resizable);
        //}

        //[Fact]
        //public void Create_DefaulSortNone()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.Null(column.Sort);
        //}

        //[Fact]
        //public void Create_DefaulDraggingNone()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.False(column.Dragging);
        //}

        //[Fact]
        //public void Create_DefaulFieldExpressionNull()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.Null(column.FieldExpression);
        //}

        //[Fact]
        //public void CreateId_FieldTypeInt()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.Equal(typeof(int), column.FieldType);
        //}

        //[Fact]
        //public void Create_FixedTypeNone()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.Equal(ColumnFixedType.None, column.FixedType);
        //}

        //[Fact]
        //public void Create_DefaultResizingFalse()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.False(column.Resizing);
        //}

        //[Fact]
        //public void CreateId_DefaultTitleId()
        //{
        //    //arrange
        //    Expression<Func<User, object>> expression = x => x.Id;

        //    //act
        //    var column = new ColumnModel<User>(expression);

        //    //assert
        //    Assert.Equal(nameof(User.Id), column.Title);
        //}
    }
}