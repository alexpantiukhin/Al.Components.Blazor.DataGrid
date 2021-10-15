using Al.Components.Blazor.AlDataGrid.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid.Model
{
    public class FilterExpression
    {
        public string ColumnUniqueName {  get; }    
        public FilterOperation Operation { get; }
        public object Value { get; }
        FilterExpressionGroupType GroupType { get; }
        public IEnumerable<FilterExpression> GroupFilterExpressions {  get; }

        private FilterExpression() { }

        /// <summary>
        /// Создание группы выражений
        /// </summary>
        /// <param name="type">Тип группы</param>
        /// <param name="expressions">Набор выражений</param>
        /// <exception cref="ArgumentNullException">Возникает, если не передать набор или передать пустой</exception>
        public FilterExpression(FilterExpressionGroupType type, IEnumerable<FilterExpression> expressions)
        {
            GroupType = type;

            if(expressions?.Any() != true)
                throw new ArgumentNullException(nameof(expressions), "Expression null or empty");
            
            // предотвращает преобразование к list с последующей возможностью изменения
            GroupFilterExpressions = expressions.AsEnumerable();
        }

        /// <summary>
        /// Создание выражения
        /// </summary>
        /// <param name="columnUniqueName">Уникальное имя столбца</param>
        /// <param name="operation">Оператор</param>
        /// <param name="value">Значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FilterExpression(string columnUniqueName, FilterOperation operation, object value)
        {
            ColumnUniqueName = columnUniqueName ?? throw new ArgumentNullException(nameof(columnUniqueName));
            Operation = operation;
            Value = value;
        } 

        public Expression GetExpression<T>(IEnumerable<ColumnModel<T>> columns)
            where T : class
        {
            Expression result;
            if (ColumnUniqueName != null)
            {
                var column = columns.FirstOrDefault(x => x.UniqueName == ColumnUniqueName);

                result = column.MemberExpression;
            }
            else
            {
                result = null;

            }

            return result;
        }
    }
}
