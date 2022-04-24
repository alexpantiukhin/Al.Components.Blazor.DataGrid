using Al.Collections;
using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

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
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id))
            {
                Sort = SortDirection.Ascending
            };

            var eventTest = new EventTestFuncTask<ColumnModel>(column,
                nameof(ColumnModel.OnUserSettingsChanged));
            var settings = new ColumnSettings(nameof(User.Id))
            {
                Sort = SortDirection.Descending
            };

            //act 
            await column.ApplySettingAsync(settings);

            //assert
            Assert.Equal(SortDirection.Descending, column.Sort);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task WidthChange()
        {
            // arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id))
            {
                Width = 200
            };
            var eventTest = new EventTestFuncTask<ColumnModel>(column,
                nameof(ColumnModel.OnUserSettingsChanged));
            var settings = new ColumnSettings(nameof(User.Id))
            {
                Width = 100
            };

            //act 
            await column.ApplySettingAsync(settings);

            //assert
            Assert.Equal(100, column.Width);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task VisibleChange()
        {
            // arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id))
            {
                Visible = true
            };
            var settings = new ColumnSettings(nameof(User.Id))
            {
                Visible = false
            };
            var eventTest = new EventTestFuncTask<ColumnModel>(column,
                nameof(ColumnModel.OnUserSettingsChanged));

            //act 
            await column.ApplySettingAsync(settings);

            //assert
            Assert.False(column.Visible);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task FixedTypeChange()
        {
            // arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id))
            {
                FixedType = ColumnFixedType.None
            };
            var settings = new ColumnSettings(nameof(User.Id))
            {
                FixedType = ColumnFixedType.Left
            };
            var eventTest = new EventTestFuncTask<ColumnModel>(column,
                nameof(ColumnModel.OnUserSettingsChanged));

            //act 
            await column.ApplySettingAsync(settings);

            //assert
            Assert.Equal(ColumnFixedType.Left, column.FixedType);
            Assert.True(eventTest.CallEvent);
        }

        [Fact]
        public async Task FilterChange()
        {
            // arrange
            var columns = new TestColumns();
            var column = new ColumnModel(columns, nameof(User.Id));
            var filter = new RequestFilter(nameof(User.Id), FilterOperation.Equal, "1");
            var settings = new ColumnSettings(nameof(User.Id))
            {
                Filter = filter
            };
            var eventTest = new EventTestFuncTask<ColumnModel>(column,
                nameof(ColumnModel.OnUserSettingsChanged));

            //act 
            await column.ApplySettingAsync(settings);

            //assert
            Assert.Equal(filter, column.Filter);
            Assert.True(eventTest.CallEvent);
        }
    }
}
