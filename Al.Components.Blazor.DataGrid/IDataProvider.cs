using Al.Components.Blazor.AlDataGrid.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Al.Components.Blazor.AlDataGrid
{
    public interface IDataProvider<T>
        where T: class
    {
        Task<IQueryable<T>> LoadData(CancellationToken token);

        Task<int> GetCount(IQueryable<T> data, CancellationToken token);

        Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token);
    }
}
