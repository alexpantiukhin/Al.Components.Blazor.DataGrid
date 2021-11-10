using Al.Components.Blazor.DataGrid.Model.Settings;
using Al.Components.Blazor.DataGrid.Tests.Data;
using Al.Components.Blazor.DataGrid.TestsData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_ApplySettings
    {
        [Fact]
        public async Task ColumnIdSetUnvisible_ColumnNotVisible()
        {
            //arrange
            bool callEvent = false;
            ColumnsModel<User> columns = Models.AddColumns(new());
            var column1 = columns.All[0].Value;
            var column2 = columns.All[1].Value;

            var settings = new List<ColumnSettings<User>>();
            settings.Add(new ColumnSettings<User> { UniqueName = nameof(User.Id), Visible = false });

            Func<Task> eventHandler = async () => callEvent = true;
            EventTest<ColumnModel<User>> eventTest = new(column1, nameof(column1.OnUserSettingsChanged), eventHandler);


            //act
            await columns.ApplySettings(settings);

            //assert
            Assert.False(column1.Visible);
            Assert.True(column2.Visible);
            Assert.True(callEvent);
        }
        [Fact]
        public async Task NoColumnInSettings_ColumnVisible()
        {
            //arrange
            bool callEvent = false;
            ColumnsModel<User> columns = Models.AddColumns(new());
            var column1 = columns.All[0].Value;
            var column2 = columns.All[1].Value;

            var settings = new List<ColumnSettings<User>>();
            settings.Add(new ColumnSettings<User> { UniqueName = "NoColumn", Visible = false });

            Func<Task> eventHandler = async () => callEvent = true;
            EventTest<ColumnModel<User>> eventTest = new(column1, nameof(column1.OnUserSettingsChanged), eventHandler);


            //act
            var result = await columns.ApplySettings(settings);

            //assert
            Assert.True(column1.Visible);
            Assert.True(column2.Visible);
            Assert.False(callEvent);
            Assert.False(result.Success);
        }
    }
}
