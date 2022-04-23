using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Interfaces;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public class TestColumns : IColumns
    {
        public bool FilterNotify { get; private set; }
        public bool FixedTypeNotify { get; private set; }
        public bool SortNotify { get; private set; }
        public bool VisibleNotify { get; private set; }

        public async Task FilterChangedNotify(ColumnModel columnModel)
        {
            FilterNotify = true;
        }

        public async Task FixedTypeChangedNotify(ColumnModel columnModel)
        {
            FixedTypeNotify = true;
        }

        public async Task SortChangedNotify(ColumnModel columnModel)
        {
            SortNotify = true;
        }

        public async Task VisibleChangedNotify(ColumnModel columnModel)
        {
            VisibleNotify = true;
        }
    }
}
