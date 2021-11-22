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
    public class FilterModel_ToggleEnabled
    {
        FilterExpression<User> filter => new(nameof(User.Id), FilterOperation.Equals, 1);
        ColumnModel<User> columnId = new(x => x.Id);
        ColumnModel<User> columnFirstName = new(x => x.FirstName);

        [Fact]
        public async Task WithoutParametersExpressionIsNull_EnableToggleEventNotCall()
        {
            // arrange
            var model = new FilterModel<User>();
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.ToggleEnabled();

            //assert
            Assert.False(callEvent);
            Assert.False(model.Enabled);
        }

        [Fact]
        public async Task WithoutParametersExpressionNotIsNull_EnableToggleEventNotCall()
        {
            // arrange
            var model = new FilterModel<User>()
            {
                FilterMode = FilterMode.Constructor
            };
            await model.SetExpression(filter);
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.ToggleEnabled();

            //assert
            Assert.True(callEvent);
            Assert.False(model.Enabled);
        }

        [Fact]
        public async Task ParameterTrueExpressionNotIsNull_EnableNotToggleEventNotCall()
        {
            // arrange
            var model = new FilterModel<User>()
            {
                FilterMode = FilterMode.Constructor
            };
            await model.SetExpression(filter);
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.ToggleEnabled(true);

            //assert
            Assert.False(callEvent);
            Assert.True(model.Enabled);
        }

        [Fact]
        public async Task ParameterFalseExpressionNotIsNull_EnableToggleEventCall()
        {
            // arrange
            var model = new FilterModel<User>()
            {
                FilterMode = FilterMode.Constructor
            };
            await model.SetExpression(filter);
            var callEvent = false;
            var eventModel = new EventTest<FilterModel<User>>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.ToggleEnabled(false);

            //assert
            Assert.True(callEvent);
            Assert.False(model.Enabled);
        }
    }
}
