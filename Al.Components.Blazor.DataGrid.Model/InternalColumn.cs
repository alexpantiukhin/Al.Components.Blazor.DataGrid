using Al.Components.Blazor.DataGrid.Model.Enums;

using System.ComponentModel;

namespace Al.Components.Blazor.DataGrid.Model
{
    public abstract class InternalColumn<T>
        where T : class
    {
        public virtual DataGridModel<T> GridModel { get; set; }

        /// <summary>
        /// Формат отображения
        /// </summary>
        /// <example>
        /// {0: dd.MM.yyyy}
        /// </example>
        public virtual string DisplayFormatStarting { get; set; }

        public virtual string ShowNameStarting { get; set; }

        public bool SortableStarting { get; set; }

        ///// <summary>
        ///// для отслеживания смены внешнего состояния
        ///// </summary>
        //bool _privateVisible = true;
        /// <summary>
        /// Показывать или нет столбец
        /// </summary>
        public virtual bool VisibleStarting { get; set; } = true;
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

        public virtual int IndexStarting { get; set; }

        public virtual bool DraggableStarting { get; set; }

        //EnumSort _privateSort;
        /// <summary>
        /// Сортировка по-умолчанию
        /// </summary>
        public virtual ListSortDirection SortStarting { get; set; }
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

        public virtual bool FilterableStarting { get; set; }

        //int _privateWidth = ColumnModel<T>.DefaultWidth;
        public virtual int WidthStarting { get; set; }
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

        public bool ResizeableStarting { get; set; }

        /// <summary>
        /// Фиксирует столбец слева или справа
        /// </summary>
        public virtual ColumnFixedType FixedTypeStarting { get; set; } = ColumnFixedType.None;



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

        //public IQueryable<T> AddFilter(IQueryable<T> data)
        //{
        //    if (Model.FilterExpression == null)
        //        return data;

        //    return data.Where(Model.FilterExpression);
        //}

        //public abstract RenderFragment GetFilterFragment();

        //public void SetFilter(Expression<Func<T, bool>> filter)
        //{
        //    Model.FilterExpression = filter;
        //}
    }
}
