using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.FilterModel
{
    public class FilterModel_SetExpressionByColumns
    {
        FilterExpression<User> filter => new(nameof(User.Id), FilterOperation.Equals, 1);
        ColumnModel<User> columnId = new(x => x.Id);
        ColumnModel<User> columnFirstName = new(x => x.FirstName);

        [Fact]
        public async Task ColumnsNull_throw()
        {
            // arrange
            var model = new FilterModel<User>
            {
                FilterMode = FilterMode.Constructor
            };

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => model.SetExpressionByColumns(null));
        }

        [Fact]
        public async Task FilteModeRow_ExpressionByColumns()
        {
            // arrange
            var model = new FilterModel<User>() { FilterMode = FilterMode.Row };
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            await columnId.FilterChange(filter);

            //act
            await model.SetExpressionByColumns(new ColumnModel<User>[] { columnId, columnFirstName });


            //assert
            Assert.True(filter.Equal(model.Expression));
            Assert.True(callEvent);
        }

        [Fact]
        public async Task FilteModeNotRow_ExpressionIsNull()
        {
            // arrange
            var model = new FilterModel<User>();
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.SetExpressionByColumns(new ColumnModel<User>[] { columnId, columnFirstName });


            //assert
            Assert.Null(model.Expression);
            Assert.False(callEvent);
        }
    }
}
