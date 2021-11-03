using Al.Components.Blazor.DataGrid.Tests.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public class ColumnModel_ChangeProperties
    {
        [Fact]
        public void VisibleSetter()
        {
            //arrange
            var column = new ColumnModel<User>(x => x.Id);

            TestHelper.TestAsynSetter(column, nameof(column.Visible), true, false, nameof(ColumnModel<User>.VisibleChange), nameof(column.OnVisibleChanged));
        }
    }
}
