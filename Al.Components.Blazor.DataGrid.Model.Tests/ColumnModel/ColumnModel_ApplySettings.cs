using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System.ComponentModel;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public class ColumnModel_ApplySettings
    {
        [Fact]
        public async Task SortChange()
        {
            // arrange
            var column = new ColumnModel<User>(x => x.Id)
            {
                Sort = ListSortDirection.Ascending
            };

            var eventTest = new EventTestFuncTask<ColumnModel<User>>(column,
                nameof(ColumnModel<User>.OnUserSettingsChanged));
            var settings = new ColumnSettings<User>()
            {
                Sort = ListSortDirection.Descending
            };

            //act 
            await column.ApplySetting(settings);

            //assert
            Assert.Equal(ListSortDirection.Descending, column.Sort);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task WidthChange()
        {
            // arrange
            var column = new ColumnModel<User>(x => x.Id)
            {
                Width = 200
            };
            var eventTest = new EventTestFuncTask<ColumnModel<User>>(column,
                nameof(ColumnModel<User>.OnUserSettingsChanged));
            var settings = new ColumnSettings<User>()
            {
                Width = 100
            };

            //act 
            await column.ApplySetting(settings);

            //assert
            Assert.Equal(100, column.Width);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task VisibleChange()
        {
            // arrange
            var column = new ColumnModel<User>(x => x.Id)
            {
                Visible = true
            };
            var settings = new ColumnSettings<User>()
            {
                Visible = false
            };
            var eventTest = new EventTestFuncTask<ColumnModel<User>>(column,
                nameof(ColumnModel<User>.OnUserSettingsChanged));

            //act 
            await column.ApplySetting(settings);

            //assert
            Assert.False(column.Visible);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task FixedTypeChange()
        {
            // arrange
            var column = new ColumnModel<User>(x => x.Id)
            {
                FixedType = ColumnFixedType.None
            };
            var settings = new ColumnSettings<User>()
            {
                FixedType = ColumnFixedType.Left
            };
            var eventTest = new EventTestFuncTask<ColumnModel<User>>(column,
                nameof(ColumnModel<User>.OnUserSettingsChanged));

            //act 
            await column.ApplySetting(settings);

            //assert
            Assert.Equal(ColumnFixedType.Left, column.FixedType);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task FilterChange()
        {
            // arrange
            var column = new ColumnModel<User>(x => x.Id);
            var filter = new FilterExpression<User>(nameof(User.Id), FilterOperation.Equals, 1);
            var settings = new ColumnSettings<User>()
            {
                Filter = filter
            };
            var eventTest = new EventTestFuncTask<ColumnModel<User>>(column,
                nameof(ColumnModel<User>.OnUserSettingsChanged));

            //act 
            await column.ApplySetting(settings);

            //assert
            Assert.Equal(filter, column.Filter);
            Assert.True(eventTest.CallEvent);
        }
    }
}
