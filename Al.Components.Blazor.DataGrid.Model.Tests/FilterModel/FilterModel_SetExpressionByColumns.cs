using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.FilterModel
{
    public class FilterModel_SetExpressionByColumns
    {
        readonly RequestFilter filter = new(nameof(User.Id), FilterOperation.Equal, "1");
        ColumnModel columnId = new(new Model.ColumnsModel(), nameof(User.Id));
        ColumnModel columnFirstName = new(new Model.ColumnsModel(), nameof(User.FirstName));

        [Fact]
        public async Task ColumnsNull_throw()
        {
            // arrange
            var model = new Model.FilterModel();

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => model.SetExpressionByColumns(null));
        }

        [Fact]
        public async Task FilteModeRow_ExpressionByColumns()
        {
            // arrange
            var model = new Model.FilterModel();
            var callEvent = false;
            var eventModel = new EventTest<Model.FilterModel>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            await columnId.FilterChange(filter);

            //act
            await model.SetExpressionByColumns(new ColumnModel[] { columnId, columnFirstName });


            //assert
            Assert.Same(filter, model.RequestFilter);
            Assert.True(callEvent);
        }

        [Fact]
        public async Task FilteModeNotRow_ExpressionIsNull()
        {
            // arrange
            var model = new Model.FilterModel();
            var callEvent = false;
            var eventModel = new EventTest<Model.FilterModel>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.SetExpressionByColumns(new ColumnModel[] { columnId, columnFirstName });


            //assert
            Assert.Null(model.RequestFilter);
            Assert.False(callEvent);
        }
    }
}
