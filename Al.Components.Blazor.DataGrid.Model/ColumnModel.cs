using Al.Components.Blazor.DataGrid.Model.Enums;

using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid.Model
{
    public class ColumnModel<T> where T : class
    {
        static Type StringType = typeof(string);

        //public event Func<Task> OnChange;
        //public event Func<Task> OnDragStarted;
        //public event Func<Task> OnDragEnded;
        public event Func<Task> OnWidthChanged;

        public const int MinWidth = 50;
        public const int DefaultWidth = 130;
        public int Index { get; set; }
        public bool Visible { get; set; } = true;
        public bool Sortable { get; set; }
        public int Width { get; private set; } = DefaultWidth;
        public string ShowName { get; set; }
        public SortType Sort { get; set; }
        public bool Resizeable { get; set; }
        public ColumnFixedType FixedType { get; set; }
        /// <summary>
        /// Столбец можно перемещать
        /// </summary>
        public bool Draggable { get; set; }
        public bool Filterable { get; set; }
        public LinkedListNode<ColumnModel<T>> ListNode { get; private set; }
        public Func<IQueryable<T>, bool, IQueryable<T>> AddSort { get; set; }
        public string UniqueName { get; }
        //public Func<IQueryable<T>, IOrderedQueryable<T>> AddFirstSort { get; init; }
        //public Func<IOrderedQueryable<T>, IOrderedQueryable<T>> AddNextSort { get; init; }
        //public Expression<Func<T, bool>> FilterExpression { get; set; }
        /// <summary>
        /// Выражение, уникально определяющее столбец
        /// </summary>
        public Expression<Func<T, object>> FieldExpression { get; }
        public Type FieldType { get; }
        /// <summary>
        /// Флаг указывающий на то, что в текущий момент столбец перемещается
        /// </summary>
        public bool Dragging { get; private set; }
        public bool Resizing { get; private set; }

        public object Component { get; }

        public readonly MemberExpression MemberExpression;
        public ColumnModel(string uniqueName, Expression<Func<T, object>> fieldExpression, object component)
        {
            UniqueName = uniqueName;

            if (fieldExpression != null)
            {
                if (fieldExpression.Body.NodeType == ExpressionType.Convert
                    && fieldExpression.Body is UnaryExpression ue
                    && ue.Operand is not null
                    && ue.Operand is MemberExpression me1)
                    MemberExpression = me1;
                else if (fieldExpression.Body.NodeType == ExpressionType.MemberAccess
                    && fieldExpression.Body is MemberExpression me2)
                    MemberExpression = me2;

                if (MemberExpression == null)
                    throw new ArgumentException("Не удалось определить тип поля", nameof(fieldExpression));

                FieldType = MemberExpression.Type;

                if (!FieldType.IsEnum && !FieldType.IsPrimitive && FieldType != StringType)
                    throw new ArgumentException("В качестве данных для столбца могут приниматься только поля примитивных типов, enum или строки",
                        nameof(fieldExpression));
            }
            Component = component;
        }

        public ColumnModel()
        {
        }

        /// <summary>
        /// Возвращает следующий за текущим столбец из видимых
        /// </summary>
        /// <returns>Null, если текущий - последний столбец среди видимых</returns>
        public ColumnModel<T> NextVisible()
        {
            while (ListNode.Next != null)
            {
                if (ListNode.Next.Value.Visible)
                    return ListNode.Next.Value;
            }

            return null;
        }

        /// <summary>
        /// Возвращает следующий за текущим столбец
        /// </summary>
        /// <returns>Null, если текущий - последний столбец</returns>
        public ColumnModel<T> Next()
        {
            return ListNode.Next?.Value;
        }

        public void SetLinkNode(LinkedListNode<ColumnModel<T>> node)
        {
            if (node == null)
                throw new ArgumentException($"Parameter {nameof(node)} is required.");

            if (node.Value != this)
                throw new ArgumentException("Node value does not equeal this");

            // делается 1 раз
            if (ListNode != null || node.Value != this) return;

            ListNode = node;
        }


        ///// <summary>
        ///// Запустить перестановку столбца
        ///// </summary>
        //public async Task ReorderStart()
        //{
        //    Dragging = true;
        //    if (OnDragStarted != null)
        //        await OnDragStarted.Invoke();
        //}

        ///// <summary>
        ///// Закончить перестановку столбца
        ///// </summary>
        //public async Task ReorderEnd()
        //{
        //    Dragging = false;
        //    if (OnDragEnded != null)
        //        await OnDragEnded.Invoke();
        //}

        //public IQueryable<T> AddFilter(IQueryable<T> data)
        //{
        //    if (FilterExpression == null)
        //        return data;

        //    return data.Where(FilterExpression);
        //}

        public async Task WidthChange(int width, bool notify)
        {
            Width = width < MinWidth ? MinWidth : width;

            if (notify && OnWidthChanged != null)
                await OnWidthChanged.Invoke();
        }
    }
}
