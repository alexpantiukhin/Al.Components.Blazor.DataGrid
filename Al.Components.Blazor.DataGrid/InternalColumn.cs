using Al.Components.Blazor.AlDataGrid.Model;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Al.Components.Blazor.AlDataGrid
{
    public abstract class InternalColumn<T> : ComponentBase
        where T : class
    {
        [CascadingParameter]
        internal DataGridModel<T> GridModel { get; set; }

        /// <summary>
        /// Формат отображения
        /// </summary>
        /// <example>
        /// {0: dd.MM.yyyy}
        /// </example>
        [Parameter]
        public string DisplayFormatStarting { get; set; }

        [Parameter]
        public string ShowNameStarting { get; set; }

        [Parameter]
        public RenderFragment<T> CellTemplate { get; set; }

        [Parameter]
        public bool SortableStarting { get; set; }

        ///// <summary>
        ///// для отслеживания смены внешнего состояния
        ///// </summary>
        //bool _privateVisible = true;
        /// <summary>
        /// Показывать или нет столбец
        /// </summary>
        [Parameter]
        public bool VisibleStarting { get; set; } = true;
        //{
        //    get => _privateVisible;
        //    set
        //    {
        //        // внутренне состояние меняется,
        //        // только если внешнее изменилось
        //        if (value == _privateVisible)
        //            return;

        //        _privateVisible = value;
        //        InternalVisible = value;
        //    }
        //}

        [Parameter]
        public int IndexStarting { get; set; }

        [Parameter]
        public bool DraggableStarting { get; set; }

        //EnumSort _privateSort;
        /// <summary>
        /// Сортировка по-умолчанию
        /// </summary>
        [Parameter]
        public EnumSort SortStarting { get; set; }
        //{
        //    get => InternalSort;
        //    set
        //    {
        //        if (value == _privateSort)
        //            return;

        //        _privateSort = value;
        //        InternalSort = value;
        //    }
        //}

        [Parameter]
        public bool FilterableStarting { get; set; }


        [Parameter]
        public RenderFragment HeadTemplate { get; set; }

        //int _privateWidth = ColumnModel<T>.DefaultWidth;
        [Parameter]
        public int WidthStarting { get; set; }
        //{
        //    get => _privateWidth;
        //    set
        //    {
        //        if (_privateWidth == value)
        //            return;

        //        if (value < ColumnModel<T>.MinWidth)
        //            _privateWidth = ColumnModel<T>.MinWidth;
        //        else
        //            _privateWidth = value;

        //        InternalWidth = _privateWidth;
        //    }
        //}

        [Parameter]
        public bool ResizeableStarting { get; set; }

        /// <summary>
        /// Фиксирует столбец слева или справа
        /// </summary>
        [Parameter]
        public EnumColumnFixedType FixedTypeStarting { get; set; } = EnumColumnFixedType.None;



        //public Type FieldType { get; protected set; }
        /// <summary>
        /// Используется клиентским скрытием/показом столбца
        /// </summary>
        //internal bool InternalVisible { get; set; }
        //internal int InternalWidth { get; set; } = ColumnModel<T>.DefaultWidth;
        //Expression<Func<T, bool>> FilterExpression { get; set; }
        //internal EnumSort InternalSort { get; set; }
        internal ColumnModel<T> Model { get; set; }
        //internal LinkedListNode<InternalColumn<T>> ListNode { get; set; }
        internal object Component { get; set; }
        //ColumnResizer<T> _resizer;
        //internal ColumnResizer<T> Resizer => _resizer ??= new ColumnResizer<T>(ListNode);
        //internal EnumResizeMode ResizeMode { get; set; }



        ///// <summary>
        ///// Установка текущего столбца после переданного
        ///// </summary>
        ///// <param name="column">Столбец</param>
        //public void SetAfterColumn(LinkedListNode<InternalColumn<T>> column)
        //{
        //    if (column.Value == this)
        //        return;

        //    var list = ListNode.List;

        //    if (column == null)
        //    {
        //        list.Remove(ListNode);
        //        list.AddLast(ListNode);
        //        return;
        //    }

        //    list.Remove(ListNode);
        //    list.AddAfter(column, ListNode);
        //}
        ///// <summary>
        ///// Установка текущего столбца после переданного
        ///// </summary>
        ///// <param name="column">Столбец</param>
        //public void SetBeforeColumn(LinkedListNode<InternalColumn<T>> column)
        //{
        //    if (column.Value == this)
        //        return;

        //    var list = ListNode.List;

        //    if (column == null)
        //    {
        //        list.Remove(ListNode);
        //        list.AddFirst(ListNode);
        //        return;
        //    }

        //    list.Remove(ListNode);
        //    list.AddBefore(column, ListNode);
        //}


        public abstract string GetStringValue(T model);

        public abstract object GetValue(T model);

        public abstract IQueryable<T> AddSort(IQueryable<T> data, bool thenOrdered);

        public IQueryable<T> AddFilter(IQueryable<T> data)
        {
            if (Model.FilterExpression == null)
                return data;

            return data.Where(Model.FilterExpression);
        }

        public abstract RenderFragment GetFilterFragment();

        public void SetFilter(Expression<Func<T, bool>> filter)
        {
            Model.FilterExpression = filter;
        }
    }
}
