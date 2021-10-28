using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.QueryableFilterExpression;

namespace ConsoleApp
{
    internal class ListDataProvider : IDataProvider<ViewModel>
    {
        readonly ListContext _data;
        public ListDataProvider(ListContext data)
        {
            _data = data;
        }
        public Task<int> GetCount(FilterExpression<ViewModel> filterExpression, CancellationToken token) =>
            Task.FromResult(_data.SubModels
                .Select(x => new ViewModel(x))
                .AsQueryable()
                .Where(filterExpression.GetExpression("x"))
                .Count());

        public Task<ViewModel[]> GetMaterializationData(IQueryable<ViewModel> data, CancellationToken token) =>
            Task.FromResult(data.ToArray());

        public Task<IQueryable<ViewModel>> LoadData(FilterExpression<ViewModel> filterExpression, CancellationToken token) =>
            Task.FromResult(_data.SubModels
                .Select(x => new ViewModel(x))
                .AsQueryable()
                .Where(filterExpression.GetExpression("x")));
    }
}
