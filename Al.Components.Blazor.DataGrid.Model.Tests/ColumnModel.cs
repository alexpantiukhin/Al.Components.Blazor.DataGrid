using Al.Components.Blazor.DataGrid.Tests.Data;

using System;
using System.Linq.Expressions;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public class ColumnModel
    {
        [Fact]
        public void CreateByUniqueName_HasUniqueName()
        {
            //arrange
            string name = "column";

            //act
            var column = new ColumnModel<User>(name);

            //assert
            Assert.Equal(name, column.UniqueName);
        }

        [Fact]
        public void CreateByExpression_HasUniqueName()
        {
            //arrange
            string propName = nameof(User.Id);
            Expression<Func<User, object>> expression = x => x.Id;

            //act
            var column = new ColumnModel<User>(expression);

            //assert
            Assert.Equal(propName, column.UniqueName);
        }

        [Fact]
        public void CreateWidthLessMinimal_WidthMinimal()
        {
            //arrange
            Expression<Func<User, object>> expression = x => x.Id;

            //act
            var column = new ColumnModel<User>(expression)
            {
                Width = ColumnModel<User>.MinWidth - 10
            };

            //assert
            Assert.Equal(ColumnModel<User>.MinWidth, column.Width);
        }


    }
}