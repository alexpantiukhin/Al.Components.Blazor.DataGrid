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
        RequestFilter filter => new(nameof(User.Id), FilterOperation.Equal, "1");
        ColumnModel columnId = new(new Model.ColumnsModel(), nameof(User.Id));
        ColumnModel columnFirstName = new(new Model.ColumnsModel(), , nameof(User.FirstName));

        [Fact]
        public async Task WithoutParametersExpressionIsNull_EnableToggleEventNotCall()
        {
            // arrange
            var model = new Model.FilterModel();
            var callEvent = false;
            var eventModel = new EventTest<Model.FilterModel>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

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
            var model = new Model.FilterModel();
            await model.SetExpression(filter);
            var callEvent = false;
            var eventModel = new EventTest<Model.FilterModel>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

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
            var model = new Model.FilterModel();
            await model.SetExpression(filter);
            var callEvent = false;
            var eventModel = new EventTest<Model.FilterModel>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

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
            var model = new Model.FilterModel();
            await model.SetExpression(filter);
            var callEvent = false;
            var eventModel = new EventTest<Model.FilterModel>(model, nameof(model.OnFilterChanged), async () => callEvent = true);

            //act
            await model.ToggleEnabled(false);

            //assert
            Assert.True(callEvent);
            Assert.False(model.Enabled);
        }
    }
}
