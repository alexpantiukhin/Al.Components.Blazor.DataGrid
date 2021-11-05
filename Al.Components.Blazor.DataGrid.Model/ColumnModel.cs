using Al.Collections;
using Al.Collections.QueryableFilterExpression;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.DataGrid.Model.Settings;

using System.ComponentModel;
using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid.Model
{
    /// <summary>
    /// Модель столбца грида
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnModel<T> : IColumn<T>
        where T : class
    {
        #region Properties

        #region Visible
        bool _visible = true;
        /// <summary>
        /// Видимость
        /// </summary>
        public bool Visible { get => _visible; init => _visible = value; }
        /// <summary>
        /// Изменяет видимость
        /// </summary>
        /// <param name="visible">флаг</param>
        public async Task VisibleChange(bool visible)
        {
            if (Visible != visible)
            {
                _visible = visible;

                if (OnVisibleChanged != null)
                    await OnVisibleChanged.Invoke();
            }
        }
        public event Func<Task>? OnVisibleChanged;
        #endregion

        #region Sortable
        bool _sortable;
        /// <summary>
        /// Возможность сортировки
        /// </summary>
        public bool Sortable { get => _sortable; set => _sortable = value; }
        /// <summary>
        /// Изменяет возможность сортировки
        /// </summary>
        /// <param name="sortable"></param>
        /// <returns></returns>
        public async Task SortableChange(bool sortable)
        {
            if (Sortable != sortable)
            {
                Sortable = sortable;

                if (OnSortableChanged != null)
                    await OnSortableChanged.Invoke();

                if (Sort != null)
                    await SortChange(null);
            }
        }
        public event Func<Task>? OnSortableChanged;
        #endregion

        #region Width
        int _width = DefaultWidth;
        /// <summary>
        /// Ширина
        /// </summary>
        public int Width { get => _width; init => _width = ColumnModel<T>.WidthCorrect(value); }

        /// <summary>
        /// Изменяет ширину
        /// </summary>
        /// <param name="width">Новая ширина</param>
        public async Task WidthChange(int width)
        {
            int newWidth = ColumnModel<T>.WidthCorrect(width);

            if (newWidth != _width)
            {
                _width = newWidth;
                if (OnWidthChanged != null)
                    await OnWidthChanged.Invoke();
            }
        }

        static int WidthCorrect(int value) => value < MinWidth ? MinWidth : value;

        /// <summary>
        /// Срабатывает после изменения ширины столбца
        /// </summary>
        public event Func<Task>? OnWidthChanged;
        #endregion

        #region Title
        string? _title;
        /// <summary>
        /// Отображаемый заголовок
        /// </summary>
        public string? Title { get => _title; init => _title = value; }

        public async Task TitleChange(string? title)
        {
            if (_title != title.Trim())
            {
                _title = title;

                if (OnTitleChanged != null)
                    await OnTitleChanged.Invoke();
            }
        }
        public event Func<Task>? OnTitleChanged;
        #endregion

        #region Sort
        ListSortDirection? _sort;
        /// <summary>
        /// Направление сортировки
        /// </summary>
        public ListSortDirection? Sort { get => _sort; init => _sort = value; }

        public async Task SortChange(ListSortDirection? sort)
        {
            if (_sort != sort)
            {
                _sort = sort;

                if (OnSortChanged != null)
                    await OnSortChanged.Invoke();
            }
        }
        public event Func<Task>? OnSortChanged;
        #endregion

        #region Resizable
        bool _resizable;
        /// <summary>
        /// Возможность менять ширину
        /// </summary>
        public bool Resizable { get => _resizable; init => _resizable = value; }
        public async Task ResizeableChange(bool resizeable)
        {
            if (_resizable != resizeable)
            {
                _resizable = resizeable;

                if (OnResizeableChanged != null)
                    await OnResizeableChanged.Invoke();
            }

        }
        public event Func<Task>? OnResizeableChanged;
        #endregion

        #region FixedType
        ColumnFixedType _fixedType = ColumnFixedType.None;
        /// <summary>
        /// Фиксация столбца справа или слева
        /// </summary>
        public ColumnFixedType FixedType { get => _fixedType; init => _fixedType = value; }
        public async Task FixedTypeChange(ColumnFixedType columnFixedType)
        {
            if (_fixedType != columnFixedType)
            {
                _fixedType = columnFixedType;

                if (OnFixedTypeChanged != null)
                    await OnFixedTypeChanged.Invoke();
            }
        }
        public event Func<Task>? OnFixedTypeChanged;
        #endregion

        #region Draggable
        bool _draggable;
        /// <summary>
        /// Возможность перемещать столбец
        /// </summary>
        public bool Draggable { get => _draggable; init => _draggable = value; }
        public async Task DraggableChange(bool draggable)
        {
            if (_draggable != draggable)
            {
                _draggable = draggable;

                if (OnDraggableChanged != null)
                    await OnDraggableChanged.Invoke();
            }
        }
        public event Func<Task>? OnDraggableChanged;
        #endregion

        #region Filterable
        bool _filterable;
        /// <summary>
        /// Возможность фильтровать по столбцу
        /// </summary>
        public bool Filterable { get => _filterable; init => _filterable = value; }
        public async Task FilterableChange(bool filterable)
        {
            if (_filterable != filterable)
            {
                _filterable = filterable;

                if (OnFilterableChanged != null)
                    await OnFilterableChanged.Invoke();
            }
        }
        public event Func<Task>? OnFilterableChanged;
        #endregion

        #region Filter
        /// <summary>
        /// Выражение фильтра по столбцу
        /// </summary>
        public FilterExpression<T>? Filter { get; private set; }
        public async Task FilterChange(FilterExpression<T>? filter)
        {
            if (filter != Filter)
            {
                Filter = filter;

                if (OnFilterChanged != null)
                    await OnFilterChanged.Invoke();
            }
        }
        public event Func<Task>? OnFilterChanged;
        #endregion

        /// <summary>
        /// Уникальное имя столбца
        /// </summary>
        public string UniqueName { get; }
        /// <summary>
        /// Выражение, уникально определяющее столбец
        /// </summary>
        public Expression<Func<T, object>>? FieldExpression { get; }
        /// <summary>
        /// Тип поля столбца
        /// </summary>
        public Type? FieldType { get; }
        /// <summary>
        /// Флаг указывающий на то, что в текущий момент столбец перемещается
        /// </summary>
        public bool Dragging { get; private set; }
        /// <summary>
        /// Флаг, указывающий на то, что в данный момент столбец меняет ширину
        /// </summary>
        public bool Resizing { get; private set; }
        #endregion


        static readonly Type StringType = typeof(string);
        public const int MinWidth = 50;
        public const int DefaultWidth = 130;
        readonly MemberExpression? MemberExpression;

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
            Title = UniqueName;
        }

        /// <summary>
        /// Столбец не привязанный к полю модели
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <param name="component"></param>
        public ColumnModel(string uniqueName)
        {
            if(string.IsNullOrWhiteSpace(uniqueName))
                throw new ArgumentNullException(nameof(uniqueName));

            UniqueName = uniqueName;
        }

        /// <summary>
        /// Конструктор по-умолчанию переопределять нельзя
        /// </summary>
        ColumnModel()
        {
        }

        /// <summary>
        /// Запуск перестановки столбца
        /// </summary>
        public async Task ReorderStart()
        {
            Dragging = true;

            if (OnDragStarted != null)
                await OnDragStarted.Invoke();
        }

        /// <summary>
        /// Закончить перестановку столбца
        /// </summary>
        public async Task ReorderEnd()
        {
            Dragging = false;

            if (OnDragEnded != null)
                await OnDragEnded.Invoke();
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
        public async Task ApplySetting(ColumnSettings<T> settings)
        {
            _sort = settings.Sort;
            _width = settings.Width;
            _visible = settings.Visible;
            _fixedType = settings.FixedType;
            Filter = settings.Filter;

            if (OnUserSettingsChanged != null)
                await OnUserSettingsChanged.Invoke();
        }

        //public event Func<Task> OnChange;
        public event Func<Task> OnDragStarted;
        public event Func<Task> OnDragEnded;
        public event Func<Task>? OnUserSettingsChanged;

    }
}
