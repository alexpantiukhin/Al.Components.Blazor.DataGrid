using Al.Components.Blazor.AlDataGrid.Model;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Al.Components.Blazor.AlDataGrid
{
    static class Helper
    {
        static Func<ColumnModel<T>, bool> GetVisibilityFilterFunc<T>() where T: class => x => x.Visible;
        /// <summary>
        /// Оставляет только отображаемые столбцы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allColumns"></param>
        /// <returns></returns>
        public static IEnumerable<LinkedListNode<ColumnModel<T>>> GetVisibility<T>(this LinkedList<ColumnModel<T>> allColumns)
            where T: class
        {
            return allColumns.Where(GetVisibilityFilterFunc<T>()).Select(x => x.ListNode);
        }
        /// <summary>
        /// Оставляет только отображаемые столбцы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allColumns"></param>
        /// <returns></returns>
        public static IEnumerable<LinkedListNode<ColumnModel<T>>> GetVisibility<T>(this IEnumerable<LinkedListNode<ColumnModel<T>>> allColumns)
            where T : class
        {
            return allColumns.Select(x => x.Value).Where(GetVisibilityFilterFunc<T>()).Select(x => x.ListNode);
        }
    }
}
