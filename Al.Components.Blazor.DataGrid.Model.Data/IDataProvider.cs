namespace Al.Components.Blazor.DataGrid.Model.Data
{
    /// <summary>
    /// Провайдер данных грида
    /// </summary>
    /// <typeparam name="T">Тип записи</typeparam>
    public interface IDataProvider<T>
        where T: class
    {
        /// <summary>
        /// Поставляет queryable-запрос
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IQueryable<T>> LoadData(CancellationToken token);

        /// <summary>
        /// Подсчитывает количество записей в запросе
        /// </summary>
        /// <param name="data">запрос</param>
        /// <param name="token">токен отмены</param>
        /// <returns>Количество записей</returns>
        Task<int> GetCount(IQueryable<T> data, CancellationToken token);

        /// <summary>
        /// Возвращает материализованные данные из запроса
        /// </summary>
        /// <param name="data">запрос</param>
        /// <param name="token">токен отмены</param>
        /// <returns>Массив данных</returns>
        Task<T[]> GetMaterializationData(IQueryable<T> data, CancellationToken token);
    }
}
