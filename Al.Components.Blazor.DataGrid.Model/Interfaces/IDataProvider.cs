﻿using Al.Components.Blazor.DataGrid.Model.Data;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IDataProvider<T>
        where T: class
    {
        Task<IQueryable<T>> LoadData(DataPaginateRequest<T> request, CancellationToken token);

        Task<int> GetCount(DataRequest<T> request, CancellationToken token);

        Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token);
    }
}
