using Al.Components.Blazor.DataGrid.Tests.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_ChangeProperties
    {

        [Fact]
        public void DragableSetter()
        {
            //arrange
            var columns = new ColumnsModel<User>();

            TestHelper.TestAsynSetter(columns, nameof(columns.Draggable),
                false, true, nameof(columns.DraggableChange),
                nameof(columns.OnDraggableChanged), true);
        }
    }
}
