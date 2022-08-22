using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Interfaces;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public class TestColumns
    {
        public bool FilterNotify { get; private set; }
        public bool FixedTypeNotify { get; private set; }
        public bool SortNotify { get; private set; }
        public bool SortIndexNotify { get; private set; }
        public bool VisibleNotify { get; private set; }

        public async Task FilterChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            FilterNotify = true;
        }

        public async Task FrozenTypeChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            FixedTypeNotify = true;
        }

        public async Task SortChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            SortNotify = true;
        }

        public async Task SortIndexChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            SortIndexNotify = true;
        }

        public async Task VisibleChangedNotify(ColumnModel columnModel, CancellationToken cancellationToken = default)
        {
            VisibleNotify = true;
        }
    }
}
