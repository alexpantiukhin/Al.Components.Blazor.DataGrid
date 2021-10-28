namespace Al.Components.Blazor.DataGrid.Model
{
    static class Helper
    {
        static Func<ColumnModel<T>, bool> GetVisibilityFilterFunc<T>() where T: class => x => x.Visible;
        ///// <summary>
        ///// Оставляет только отображаемые столбцы
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="allColumns"></param>
        ///// <returns></returns>
        //public static IEnumerable<LinkedListNode<ColumnModel<T>>> GetVisibility<T>(this LinkedList<ColumnModel<T>> allColumns)
        //    where T: class
        //{
        //    return allColumns.Where(GetVisibilityFilterFunc<T>()).Select(x => x.Node);
        //}
        ///// <summary>
        ///// Оставляет только отображаемые столбцы
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="allColumns"></param>
        ///// <returns></returns>
        //public static IEnumerable<LinkedListNode<ColumnModel<T>>> GetVisibility<T>(this IEnumerable<LinkedListNode<ColumnModel<T>>> allColumns)
        //    where T : class
        //{
        //    return allColumns.Select(x => x.Value).Where(GetVisibilityFilterFunc<T>()).Select(x => x.Node);
        //}
    }
}
