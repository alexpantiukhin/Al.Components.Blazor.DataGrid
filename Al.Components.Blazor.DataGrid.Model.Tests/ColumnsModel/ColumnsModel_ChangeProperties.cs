using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests.ColumnsModel
{
    public class ColumnsModel_ChangeProperties
    {

        [Fact]
        public void DragableSetter()
        {
            //arrange
            var columns = new Model.ColumnsModel();

            TestHelper.TestAsynSetter(columns, nameof(columns.Draggable),
                false, true, nameof(columns.DraggableChange),
                nameof(columns.OnDraggableChanged), true);
        }
    }
}
