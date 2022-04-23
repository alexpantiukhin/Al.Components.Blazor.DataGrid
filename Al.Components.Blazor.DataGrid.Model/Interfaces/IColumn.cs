using Al.Collections;
using Al.Components.Blazor.DataGrid.Model.Enums;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumn
    {
        /// <summary>
        /// Видимость столбца
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Возможность сортировки
        /// </summary>
        bool Sortable { get; }

        /// <summary>
        /// Ширина
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Отображаемый заголовок
        /// </summary>
        string? Title { get; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        SortDirection? Sort { get; }

        /// <summary>
        /// Возможность менять ширину
        /// </summary>
        bool Resizable { get; }

        /// <summary>
        /// Фиксация столбца справа или слева
        /// </summary>
        ColumnFixedType FixedType { get; }

        /// <summary>
        /// Возможность фильтровать по столбцу
        /// </summary>
        bool Filterable { get; }

        /// <summary>
        /// Выражение фильтра по столбцу
        /// </summary>
        RequestFilter? Filter { get; }
        
        /// <summary>
        /// Уникальное имя столбца
        /// </summary>
        string UniqueName { get; }
        
        /// <summary>
        /// Шаблон для заголовка столбца. 
        /// Если в компоненте указан Template, то будет проигнорирован
        /// </summary>
        string? HeaderComponentTypeName { get; }

        /// <summary>
        /// Шаблон для ячейки. 
        /// Если в компоненте указан Template, то будет проигнорирован
        /// </summary>
        string? CellComponentTypeName { get; }
    }
}
