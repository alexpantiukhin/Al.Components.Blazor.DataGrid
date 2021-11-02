using Al.Collections;
using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Settings;

using System.ComponentModel;
using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель столбца грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnModel<T> where T : class
    {
        static readonly Type StringType = typeof(string);
        public const int MinWidth = 50;
        public const int DefaultWidth = 130;
        /// <summary>
        /// Видимость
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// Возможность сортировки
        /// </summary>
        public bool Sortable { get; set; }
        /// <summary>
        /// Ширина
        /// </summary>
        public int Width { get; private set; } = DefaultWidth;
        /// <summary>
        /// Отображаемый заголовок
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Направление сортировки
        /// </summary>
        public ListSortDirection? Sort { get; set; }
        /// <summary>
        /// Возможность менять ширину
        /// </summary>
        public bool Resizeable { get; set; }
        /// <summary>
        /// Фиксация столбца справа или слева
        /// </summary>
        public ColumnFixedType FixedType { get; set; }
        /// <summary>
        /// Столбец можно перемещать
        /// </summary>
        public bool Draggable { get; set; }
        /// <summary>
        /// Возможность фильтровать по столбцу
        /// </summary>
        public bool Filterable { get; set; }
        /// <summary>
        /// Выражение фильтра по столбцу
        /// </summary>
        public FilterExpression<T> FilterExpression { get; set; }
        /// <summary>
        /// Узел сортируемой коллекции столбцов
        /// </summary>
        public OrderableDictionaryNode<string, ColumnModel<T>> Node {get; private set;}
        /// <summary>
        /// Уникальное имя столбца
        /// </summary>
        public string UniqueName { get; }
        /// <summary>
        /// Выражение, уникально определяющее столбец
        /// </summary>
        public Expression<Func<T, object>> FieldExpression { get; }
        /// <summary>
        /// Тип поля столбца
        /// </summary>
        public Type FieldType { get; }
        /// <summary>
        /// Флаг указывающий на то, что в текущий момент столбец перемещается
        /// </summary>
        public bool Dragging { get; private set; }
        /// <summary>
        /// Флаг, указывающий на то, что в данный момент столбец меняет ширину
        /// </summary>
        public bool Resizing { get; private set; }
        /// <summary>
        /// Возвращает следующий за текущим столбец
        /// </summary>
        /// <returns>Null, если текущий - последний столбец</returns>
        public ColumnModel<T>? Next =>
            Node.Next?.Item;


        readonly MemberExpression MemberExpression;


        /// <summary>
        /// Столбец привязанный к полю модели
        /// </summary>
        /// <param name="fieldExpression">Выражение поля модели</param>
        /// <param name="component">компонент столбца</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданное выражение null </exception>
        /// <exception cref="ArgumentException">Выбрасывается, если из варежения не удаётся вывести поле модели</exception>
        public ColumnModel(Expression<Func<T, object>> fieldExpression)
        {
            if (fieldExpression is null)
                throw new ArgumentNullException(nameof(fieldExpression));

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
                
                UniqueName = MemberExpression.Member.Name;
        }

        /// <summary>
        /// Столбец не привязанный к полю модели
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <param name="component"></param>
        public ColumnModel(string uniqueName)
        {
            UniqueName = uniqueName;
        }

        /// <summary>
        /// Конструктор по-умолчанию использовать нельзя
        /// </summary>
        ColumnModel()
        {
        }

        /// <summary>
        /// Возвращает следующий за текущим столбец из видимых
        /// </summary>
        /// <returns>Null, если текущий - последний столбец среди видимых</returns>
        public ColumnModel<T>? NextVisible()
        {
            while (Node.Next != null)
            {
                if (Node.Next.Item.Visible)
                    return Node.Next.Item;
            }

            return null;
        }

        public void SetNode(OrderableDictionaryNode<string, ColumnModel<T>> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Item != this)
                throw new ArgumentException("Node value does not equeal this");

            // делается 1 раз
            if (Node != null) return;

            Node = node;
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

        /// <summary>
        /// Изменить ширину
        /// </summary>
        /// <param name="width">Новая ширина</param>
        /// <param name="notify">Уведомить об изменении ширины</param>
        public async Task WidthChange(int width, bool notify)
        {
            Width = width < MinWidth ? MinWidth : width;

            if (notify && OnWidthChanged != null)
                await OnWidthChanged.Invoke();
        }

        /// <summary>
        /// Получает значение поля указанного столбца для переданного экземпляра
        /// </summary>
        /// <param name="model">Экземпляр</param>
        public object? GetColumnValue(T model) =>
            FieldExpression?.Compile()?.Invoke(model);

        /// <summary>
        /// Применить пользовательские настройки
        /// </summary>
        /// <param name="settings">Настройки</param>
        public void ApplySetting(ColumnSettings<T> settings)
        {
            Sort = settings.Sort;
            Width = settings.Width;
            Visible = settings.Visible;
            FixedType = settings.FixedType;
            FilterExpression = settings.FilterExpression;
        }


        //public event Func<Task> OnChange;
        //public event Func<Task> OnDragStarted;
        //public event Func<Task> OnDragEnded;
        /// <summary>
        /// Срабатывает после изменения ширины столбца
        /// </summary>
        public event Func<Task>? OnWidthChanged;

    }
}
