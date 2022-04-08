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
    public class FilterModel_SetExpression
    {
        readonly FilterExpression<User> filter = new (nameof(User.Id), FilterOperation.Equals, 1);
        [Fact]
        public async Task FilteModeConstructor_NewExpression()
        {
            // arrange
            var model = new FilterModel<User>
            {
                FilterMode = FilterMode.Constructor
            };
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.SetExpression(filter);


            //assert
            Assert.Equal(filter, model.Expression);
            Assert.True(callEvent);
        }

        [Fact]
        public async Task FilteModeNotConstructor_ExpressionIsNull()
        {
            // arrange
            var model = new FilterModel<User>();
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.SetExpression(filter);


            //assert
            Assert.Null(model.Expression);
            Assert.False(callEvent);
        }
    }
}
